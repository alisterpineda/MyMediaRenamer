using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MetadataExtractor;
using MyMediaRenamer.Core;
using MyMediaRenamer.Gui.Utilities;
using MyMediaRenamer.Gui.Utilities.WindowService;

namespace MyMediaRenamer.Gui.ViewModels
{
    public class MediaFileViewModel : BaseViewModel
    {
        #region Members
        private string _initialFilePath { get; set; }
        #endregion

        #region Constructors

        public MediaFileViewModel(string filePath)
        {
            MediaFile = new MediaFile(filePath);

            ShowMetadataCommand = new RelayCommand(x => DoShowMetadata());

            MediaFile.PropertyChanged += MediaFile_PropertyChanged;
        }

        #endregion

        #region Properties

        public  MediaFile MediaFile { get; }

        public string InitialFilePath
        {
            get => _initialFilePath;
            set
            {
                if (value == InitialFilePath)
                    return;

                _initialFilePath = value;
                OnPropertyChanged(nameof(InitialFilePath));
            }
        }

        public string FilePath
        {
            get => MediaFile.FilePath;
            set => MediaFile.FilePath = value;
        }
        public string FileName => MediaFile.FileName;
        public IReadOnlyCollection<Directory> MetadataDirectories => MediaFile.MetadataDirectories;
        public string ErrorMessage => MediaFile.ErrorMessage;

        public MediaFileStatus Status
        {
            get => MediaFile.Status;
            set => MediaFile.Status = value;
        }

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

                if (Status == MediaFileStatus.Done)
                    toolTip.Append($"Initial File Path:\n{InitialFilePath}\n\n---\n\n");
                else if (Status == MediaFileStatus.Error && !string.IsNullOrEmpty(ErrorMessage))
                    toolTip.Append($"Error:\n\n{ErrorMessage}\n\n---\n\n");

                toolTip.Append(FilePath);
                return toolTip.ToString();
            }
        }

        private MetadataViewerWindowService MetadataViewerWindowService { get; } = new MetadataViewerWindowService();

        #region ICommand
        public ICommand ShowMetadataCommand { get; }
        #endregion

        #endregion

        #region Methods

        public void Reload()
        {
            InitialFilePath = FilePath;
            Status = MediaFileStatus.Normal;
        }

        #region ICommand Methods

        public void DoShowMetadata()
        {
            MetadataViewerWindowService.Show(new MetadataViewerViewModel(this));
        }

        #endregion

        #region Event Handlers

        private void MediaFile_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MediaFile.FilePath))
            {
                OnPropertyChanged(nameof(FilePath));
                OnPropertyChanged(nameof(FileName));
            }
            else if (e.PropertyName == nameof(MediaFile.Status))
            {
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(StatusIcon));
                OnPropertyChanged(nameof(ToolTip));
            }
        } 
        #endregion


        #endregion
    }
}
