using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Text;
using MetadataExtractor;
using MetadataExtractor.Util;
using MyMediaRenamer.Core.Annotations;
using Directory = MetadataExtractor.Directory;

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

        private string _initialFilePath;
        private string _filePath;
        private FileType? _fileType;
        private IReadOnlyList<Directory> _metadataDirectories;
        private MediaFileStatus _status;
        private string _errorMessage;
        
        #endregion

        #region Constructors

        public MediaFile(string filePath)
        {
            InitialFilePath = filePath;
        }
        #endregion

        #region Properties

        public string InitialFilePath
        {
            get => _initialFilePath;
            private set
            {
                if (value == _initialFilePath)
                    return;

                _initialFilePath = value;
                OnPropertyChanged(nameof(InitialFilePath));

                FilePath = InitialFilePath;
            }
        }

        public string FilePath
        {
            get => _filePath;
            private set
            {
                if (value == _filePath)
                    return;

                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
                OnPropertyChanged(nameof(FileDirectory));
                OnPropertyChanged(nameof(FileName));
            }
        }

        public string FileDirectory => Path.GetDirectoryName(FilePath);
        public string FileName => Path.GetFileName(FilePath);

        public FileType FileType
        {
            get
            {
                if (!_fileType.HasValue)
                {
                    using (var stream = GetStream())
                        _fileType = FileTypeDetector.DetectFileType(stream);
                }

                return _fileType.GetValueOrDefault();
            }
        }

        public IFileSystem FileSystem { get; set; } = new FileSystem();

        public IReadOnlyList<Directory> MetadataDirectories
        {
            get
            {
                if (_metadataDirectories == null)
                {
                    using (var stream = GetStream())
                        MetadataDirectories = ImageMetadataReader.ReadMetadata(stream);
                }

                return _metadataDirectories;
            }
            set
            {
                if (value == _metadataDirectories)
                    return;

                _metadataDirectories = value;
                OnPropertyChanged(nameof(MetadataDirectories));
            }
        }

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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (value == _errorMessage)
                    return;

                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }



        #endregion

        #region Methods

        public void Rename(string newFilePath)
        {
            string target = newFilePath;
            int i = 0;

            try
            {
                Status = MediaFileStatus.InProgress;

                while (FileSystem.File.Exists(target))
                {
                    i++;
                    target = Path.Combine( Path.GetDirectoryName(newFilePath), Path.GetFileNameWithoutExtension(newFilePath) + $" ({i})" + Path.GetExtension(newFilePath));
                }

                FileSystem.File.Move(FilePath, target);

                FilePath = target;
                Status = MediaFileStatus.Done;
            }
            catch (Exception e)
            {
                Status = MediaFileStatus.Error;
                ErrorMessage = e.Message;
            }
        }

        public void Reload()
        {
            InitialFilePath = FilePath;
            Status = MediaFileStatus.Normal;
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
