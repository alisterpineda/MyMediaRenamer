namespace MyMediaRenamer.Core.FilePathTags
{
    public class TextTag : BaseTag
    {
        #region Constructors

        public TextTag(string tagOptionsString = null) : base(tagOptionsString)
        {
        }

        #endregion

        #region Properties

        public string Text { get; set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            TextTag other = obj as TextTag;
            if (other == null)
                return false;
            return Text.Equals(other.Text);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = base.GetHashCode();
                hash = hash * 13 + Text.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"Text :: '{Text}'";
        }

        protected override string GenerateString(Renamer renamer, MediaFile mediaFile)
        {
            return Text;
        }

        #endregion
    }
}
