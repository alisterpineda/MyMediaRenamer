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

        public override string ToString()
        {
            return $"[TextTag :: Text='{Text}'{GetBasePartialToString()}]";
        }

        protected override string GenerateString(Renamer renamer, MediaFile mediaFile)
        {
            return Text;
        }

        #endregion
    }
}
