using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyMediaRenamer.Core.FilePathTags
{
    public enum IncrementReference
    {
        File,
        Directory
    }

    public class IncrementTag : BaseTag
    {
        #region Members

        private IncrementReference _incrementReference;
        private uint _fileCounter = 0;
        private Dictionary<string, uint> _directoryCounter = new Dictionary<string, uint>();

        #endregion

        #region Constructors
        public IncrementTag(string tagOptionsString = null) : base(tagOptionsString) { }
        #endregion

        #region Properties

        public IncrementReference Reference { get; set; }

        public uint Start { get; set; }

        public uint Step { get; set; } = 1;

        public string Format { get; set; } = "G";

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is IncrementTag other))
                return false;

            return base.Equals(obj) &&
                   Reference.Equals(other.Reference) &&
                   Start.Equals(other.Start) &&
                   Step.Equals(other.Step) &&
                   Format.Equals(other.Format);
        }

        public override string ToString()
        {
            return $"IncrementTag :: Reference='{Reference}',Start='{Start}',Step='{Step}',Format='{Format}'" + GetBasePartialToString();
        }

        protected override string GenerateString(Renamer renamer, MediaFile mediaFile)
        {
            switch (Reference)
            {
                case IncrementReference.File:
                    return GenerateString_PerFile(renamer, mediaFile);
                case IncrementReference.Directory:
                    return GenerateString_PerDirectory(renamer, mediaFile);
                default:
                    return null;
            }
        }

        private string GenerateString_PerFile(Renamer renamer, MediaFile mediaFile)
        {
            return (_fileCounter++ * Step + Start).ToString(Format);
        }

        private string GenerateString_PerDirectory(Renamer renamer, MediaFile mediaFile)
        {
            var fullPath = Path.GetFullPath(mediaFile.FileDirectory);
            if (!_directoryCounter.ContainsKey(fullPath))
            {
                _directoryCounter[fullPath] = 0;
            }

            return (_directoryCounter[fullPath]++ * Step + Start).ToString(Format);
        }

        #endregion
    }
}
