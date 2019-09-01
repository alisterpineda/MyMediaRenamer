using System.Text;
using MyMediaRenamer.Core;

namespace MyMediaRenamer.Gui.ViewModels
{
    public class MediaFileViewModel : BaseViewModel
    {
        #region Members
        #endregion

        #region Constructors

        public MediaFileViewModel(string filePath)
        {
            MediaFile = new MediaFile(filePath);
            MediaFile.PropertyChanged += MediaFile_PropertyChanged;
        }

        #endregion

        #region Properties

        public  MediaFile MediaFile { get; }

        public string FilePath => MediaFile.FilePath;
        public string FileName => MediaFile.FileName;
        public string ErrorMessage => MediaFile.ErrorMessage;

        public MediaFileStatus Status => MediaFile.Status;

        public string StatusIcon
        {
            get
            {
                switch (Status)
                {
                    case MediaFileStatus.Normal:
                        return "";
                    case MediaFileStatus.InProgress:
                        return "\uf110";
                    case MediaFileStatus.Done:
                        return "\uf00c";
                    case MediaFileStatus.Error:
                        return "\uf071";
                    case MediaFileStatus.Outdated:
                        return "\uf00c";
                    default:
                        return "\uf071";
                }
            }
        }

        public string ToolTip
        {
            get
            {
                StringBuilder toolTip = new StringBuilder();

                if (Status == MediaFileStatus.Error && !string.IsNullOrEmpty(ErrorMessage))
                    toolTip.Append($"Error:\n\n{ErrorMessage}\n\n---\n\n");

                toolTip.Append(FilePath);
                return toolTip.ToString();
            }
        }

        #endregion

        #region Methods
        private void MediaFile_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(StatusIcon));
                OnPropertyChanged(nameof(ToolTip));
            }
        }
        #endregion
    }
}
