using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyMediaRenamer.Core.Annotations;
using MyMediaRenamer.Core.FilePathTags;

namespace MyMediaRenamer.Core
{
    public class Renamer : ICloneable, INotifyPropertyChanged
    {
        #region Members

        private bool _preserveExtension = true;

        #endregion

        #region Properties

        public bool PreserveExtension
        {
            get => _preserveExtension;
            set
            {
                if (value == PreserveExtension)
                    return;

                _preserveExtension = value;

                OnPropertyChanged(nameof(PreserveExtension));
            }
        }

        #endregion

        #region Methods

        public async void ExecuteAll(IEnumerable<MediaFile> mediaFiles, IEnumerable<BaseTag> filePathTags)
        {
            try
            {
                await Task.Run(() =>
                {
                    foreach (var mediaFile in mediaFiles)
                    {
                        ExecuteSingle(mediaFile, filePathTags);
                    }
                });
            }
            catch (Exception) { }
        }

        public void ExecuteSingle(MediaFile mediaFile, IEnumerable<BaseTag> filePathTags)
        {
            string newFilePath = GetNewFilePath(mediaFile, filePathTags);
            mediaFile.Rename(newFilePath);
        }

        public string GetNewFilePath(MediaFile mediaFile, IEnumerable<BaseTag> filePathTags)
        {
            string newFilePath = string.Empty;

            foreach (var filePathTag in filePathTags)
            {
                newFilePath += filePathTag.GetString(this, mediaFile);
            }

            if (PreserveExtension)
                newFilePath += mediaFile.FileExtension;

            return Path.Combine(mediaFile.FileDirectory, newFilePath);
        }

        public object Clone()
        {
            return MemberwiseClone();
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
