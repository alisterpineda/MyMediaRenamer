using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using MyMediaRenamer.Core.Annotations;

namespace MyMediaRenamer.Core
{
    public enum MediaFileStatus
    {
        Normal,
        InProgress,
        Done,
        Outdated,
        Error
    }

    public class MediaFile : INotifyPropertyChanged
    {
        #region Members

        private MediaFileStatus _status;
        #endregion
        #region Constructors

        public MediaFile(string filePath)
        {
            FilePath = filePath;
        }
        #endregion

        #region Properties

        public string FilePath { get; }

        public string FileName => Path.GetFileName(FilePath);

        public MediaFileStatus Status
        {
            get => _status;
            set
            {
                if (value == _status)
                    return;

                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
