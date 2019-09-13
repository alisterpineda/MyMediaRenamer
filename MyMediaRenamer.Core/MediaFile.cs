using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif.Makernotes;
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
            FilePath = filePath;
        }
        #endregion

        #region Properties

        public string FilePath
        {
            get => _filePath;
            set
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
        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FilePath);
        public string FileExtension => Path.GetExtension(FilePath);

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

                OnRenamed(FilePath, target);

                FilePath = target;
                Status = MediaFileStatus.Done;
                
            }
            catch (Exception e)
            {
                ReportError(e.Message);
            }
        }

        public Stream GetStream()
        {
            return FileSystem.File.OpenRead(FilePath);
        }

        public void ReportError(string errorMessage)
        {
            Status = MediaFileStatus.Error;
            ErrorMessage = errorMessage;
            OnErrorReported(errorMessage);
        }

        #endregion

        public event EventHandler<MediaFileRenamedArgs> Renamed;
        public event EventHandler<MediaFileErrorReportedArgs> ErrorReported;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnRenamed(string oldFilePath, string newFilePath)
        {
            Renamed?.Invoke(this, new MediaFileRenamedArgs(oldFilePath, newFilePath));
        }

        protected virtual void OnErrorReported(string errorMessage)
        {
            ErrorReported?.Invoke(this, new MediaFileErrorReportedArgs(errorMessage));
        }
    }

    public class MediaFileRenamedArgs : EventArgs
    {
        #region Constructors

        public MediaFileRenamedArgs(string oldFilePath, string newFilePath)
        {
            OldFilePath = string.Copy(oldFilePath);
            NewFilePath = string.Copy(newFilePath);
        }

        #endregion

        #region Properties

        public string OldFilePath { get; }
        public string NewFilePath { get; }

        #endregion
    }

    public class MediaFileErrorReportedArgs : EventArgs
    {
        #region Constructors

        public MediaFileErrorReportedArgs(string errorMessage)
        {
            ErrorMessage = string.Copy(errorMessage);
        }

        #endregion

        #region Properties

        public string ErrorMessage { get; }

        #endregion
    }
}
