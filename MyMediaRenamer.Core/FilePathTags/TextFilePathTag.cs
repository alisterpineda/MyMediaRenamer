using System;
using System.Collections.Generic;
using System.Text;

namespace MyMediaRenamer.Core.FilePathTags
{
    public class TextFilePathTag : BaseFilePathTag
    {
        #region Constructors

        public TextFilePathTag(string tagOptionsString = null, MediaRenamer parent = null) : base(tagOptionsString, parent)
        {
        }

        #endregion

        #region Properties

        public string Text { get; set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            TextFilePathTag other = obj as TextFilePathTag;
            if (other == null)
                return false;
            return Text.Equals(other.Text);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Text : '{Text}'";
        }

        protected override string GenerateString(MediaFile mediaFile)
        {
            return Text;
        }

        #endregion
    }
}
