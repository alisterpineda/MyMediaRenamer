using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
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

        public IFileSystem FileSystem { get; set; } = new FileSystem();

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

        #region Methods

        public void Rename(string newFilePath)
        {
            string target = newFilePath;
            int i = 0;

            while (FileSystem.File.Exists(target))
            {
                i++;
                target = Path.GetFileNameWithoutExtension(newFilePath) + $" ({i})" + Path.GetExtension(newFilePath);
            }

            FileSystem.File.Move(FilePath, target);
        }

        public Stream GetStream()
        {
            return FileSystem.File.OpenRead(FilePath);
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
