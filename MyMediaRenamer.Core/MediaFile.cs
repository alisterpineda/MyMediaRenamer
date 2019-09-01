using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;
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
                _initialFilePath = value;
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
                    _fileType = FileTypeDetector.DetectFileType(GetStream());
                return _fileType.GetValueOrDefault();
            }
        }

        public IFileSystem FileSystem { get; set; } = new FileSystem();

        public IReadOnlyList<Directory> MetadataDirectories
        {
            get
            {
                if (_metadataDirectories == null)
                    MetadataDirectories = ImageMetadataReader.ReadMetadata(GetStream());

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

                Status = MediaFileStatus.Done;
            }
            catch (Exception)
            {
                Status = MediaFileStatus.Error;
            }
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
