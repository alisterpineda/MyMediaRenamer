using System;

namespace MyMediaRenamer.Core.FilePathTags
{
    public enum NameTagType
    {
        Name,
        Extension,
        Full
    }

    public class NameTag : BaseTag
    {
        #region Members

        #endregion

        #region Constructors

        public NameTag(string tagOptionsString = null) : base(tagOptionsString) { }

        #endregion

        #region Properties

        public NameTagType Type { get; set; } = NameTagType.Name;

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            NameTag other = obj as NameTag;
            if (other == null)
                return false;
            return Type.Equals(other.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = base.GetHashCode();
                hash = hash * 13 + Type.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"NameTag :: '{Type}'";
        }

        protected override string GenerateString(Renamer renamer, MediaFile mediaFile)
        {
            switch (Type)
            {
                case NameTagType.Name:
                    return mediaFile.FileNameWithoutExtension;
                case NameTagType.Extension:
                    return mediaFile.FileExtension.Replace(".", "");
                case NameTagType.Full:
                    return mediaFile.FileName;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion
    }
}
