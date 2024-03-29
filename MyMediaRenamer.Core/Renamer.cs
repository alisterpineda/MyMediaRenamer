﻿using System;
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

        private bool _dryRun = false;
        private bool _preserveExtension = true;
        private bool _skipOnNullTag = false;
        private string _nullTagString = "null";

        #endregion

        #region Properties

        public bool DryRun
        {
            get => _dryRun;
            set
            {
                if (value == _dryRun)
                    return;

                _dryRun = value;

                OnPropertyChanged(nameof(DryRun));
            }
        }

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

        public bool SkipOnNullTag
        {
            get => _skipOnNullTag;
            set
            {
                if (value == SkipOnNullTag)
                    return;

                _skipOnNullTag = value;

                OnPropertyChanged(nameof(SkipOnNullTag));
            }
        }

        public string NullTagString
        {
            get => _nullTagString;
            set
            {
                if (value == NullTagString)
                    return;

                _nullTagString = value;

                OnPropertyChanged(nameof(NullTagString));
            }
        }

        #endregion

        #region Methods

        public void Execute(IEnumerable<MediaFile> mediaFiles, IEnumerable<BaseTag> filePathTags)
        {
            foreach (var mediaFile in mediaFiles)
            {
                ExecuteSingle(mediaFile, filePathTags);
            }
        }

        public async void AsyncExecute(IEnumerable<MediaFile> mediaFiles, IEnumerable<BaseTag> filePathTags)
        {
            await Task.Run(() =>
                {
                    foreach (var mediaFile in mediaFiles)
                    {
                        ExecuteSingle(mediaFile, filePathTags);
                    }
                });
        }

        public void ExecuteSingle(MediaFile mediaFile, IEnumerable<BaseTag> filePathTags)
        {
            if(TryGetNewFilePath(mediaFile, filePathTags, out string newFilePath))
                mediaFile.Rename(newFilePath, DryRun);
        }

        public bool TryGetNewFilePath(MediaFile mediaFile, IEnumerable<BaseTag> filePathTags, out string newFilePath)
        {
            newFilePath = string.Empty;

            foreach (var filePathTag in filePathTags)
            {
                string tagString = filePathTag.GetString(this, mediaFile);

                if (tagString == null)
                {
                    if (SkipOnNullTag)
                    {
                        mediaFile.ReportError($"Tag '{filePathTag}' could not produce a valid string!");
                        return false;
                    }
                    else
                    {
                        newFilePath += NullTagString;
                    }
                }
                else
                {
                    newFilePath += tagString;
                }
            }

            if (PreserveExtension)
                newFilePath += mediaFile.FileExtension;

            newFilePath = Path.Combine(mediaFile.FileDirectory, newFilePath);
            return true;
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
