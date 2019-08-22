﻿using MyMediaRenamer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMediaRenamer.Gui
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

        public MediaFileStatus Status => MediaFile.Status;

        public string StatusIcon
        {
            get
            {
                switch (Status)
                {
                    case MediaFileStatus.Normal:
                        return "\uf067";
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

        #endregion

        #region Methods
        private void MediaFile_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
                OnPropertyChanged(nameof(Status));
        }
        #endregion
    }
}
