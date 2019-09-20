using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MyMediaRenamer.Core.FilePathTags;

namespace MyMediaRenamer.Core
{
    public static class PatternParser
    {
        #region Members

        private const string missingTokenOpener = "Missing closing '<'";
        private const string missingTokenCloser = "Missing closing '>'";

        private static readonly Regex _regexCustomToken = new Regex("^(.*)\\|(.*)");
        private static readonly Dictionary<string, Func<string, BaseTag>> FilePathTagLookup = new Dictionary<string, Func<string, BaseTag>>
        {
            { "datetime", (tagOptionsString) => new DateTimeTag(tagOptionsString) },
            { "hash", (tagOptionsString) => new HashTag(tagOptionsString) },
            { "inc", (tagOptionsString) => new IncrementTag(tagOptionsString) },
            { "meta", (tagOptionsString) => new MetadataTag(tagOptionsString) },
            { "name", (tagOptionsString) => new NameTag(tagOptionsString) },
            // Shortcuts
            { "ext", (tagOptionsString) => new NameTag(tagOptionsString){Type = NameTagType.Extension} },
        };

        #endregion

        #region Methods

        public static List<BaseTag> Parse(string pattern)
        {
            List<BaseTag> tags = new List<BaseTag>();
            StringBuilder buffer = new StringBuilder();
            bool tagMode = false;

            if (string.IsNullOrEmpty(pattern))
                return tags;

            for (int i = 0; i < pattern.Length; i++)
            {
                if (tagMode)
                {
                    if (pattern[i] == '>')
                    {
                        tagMode = false;
                        tags.Add(GetFilePathTag(buffer.ToString()));
                        buffer.Clear();

                    }
                    else if (i == pattern.Length - 1)
                        throw new FormatException(missingTokenCloser);
                    else
                        buffer.Append(pattern[i]);
                }
                else
                {
                    if (pattern[i] == '<')
                    {
                        if (i == pattern.Length - 1)
                            throw new FormatException(missingTokenCloser);

                        tagMode = true;

                        if (buffer.Length > 0)
                            tags.Add(new TextTag{Text = buffer.ToString()});
                        buffer.Clear();
                    }
                    else if (pattern[i] == '>')
                        throw new FormatException(missingTokenOpener);
                    else
                        buffer.Append(pattern[i]);
                }
            }

            if (buffer.Length > 0)
                tags.Add(new TextTag{Text = buffer.ToString()});

            return tags;
        }

        private static BaseTag GetFilePathTag(string tagPattern, Renamer renamer = null)
        {
            string tagType = tagPattern;
            string tagOptionsString = String.Empty;

            Match match = _regexCustomToken.Match(tagPattern);
            if (match.Success)
            {
                tagType = match.Groups[1].ToString();
                tagOptionsString = match.Groups[2].ToString();
            }

            try
            {
                return FilePathTagLookup[tagType](tagOptionsString);
            }
            catch (KeyNotFoundException)
            {
                throw new FormatException($"Invalid tag type: '{tagType}'");
            }
        }

        #endregion
    }
}
