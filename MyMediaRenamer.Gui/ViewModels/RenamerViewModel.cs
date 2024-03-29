﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using MyMediaRenamer.Gui.Utilities;

namespace MyMediaRenamer.Gui.ViewModels
{
    public class RenamerViewModel : BaseViewModel
    {
        #region Members

        private Renamer _renamer = new Renamer();
        private Renamer _backup;

        #endregion

        #region Constructors

        public RenamerViewModel()
        {
            OkCommand = new RelayCommand(x => DoEndEdit(x));
            CancelCommand = new RelayCommand(x => DoCancelEdit(x));

            _renamer.PropertyChanged += _renamer_PropertyChanged;
        }

       
        #endregion

        #region Properties

        public bool PreserveExtension
        {
            get => _renamer.PreserveExtension;
            set => _renamer.PreserveExtension = value;
        }

        public bool SkipOnNullTag
        {
            get => _renamer.SkipOnNullTag;
            set => _renamer.SkipOnNullTag = value;
        }

        public string NullTagString
        {
            get => _renamer.NullTagString;
            set => _renamer.NullTagString = value;
        }

        #region ICommands
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #endregion

        #region Methods

        public void ExecuteAll(IEnumerable<MediaFileViewModel> mediaFiles, IEnumerable<BaseTag> Tags)
        {
            _renamer.AsyncExecute(mediaFiles.Select(x => x.MediaFile), Tags);
        }

        #region ICommand Methods

        internal void BeginEdit()
        {
            _backup = (Renamer)_renamer.Clone();
        }

        private void EndEdit()
        {
            _backup = null;

        }

        private void CancelEdit()
        {
            _renamer = _backup;
        }

        private void DoEndEdit(object x)
        {
            EndEdit();
            ((Window)x).Close();
        }

        private void DoCancelEdit(object x)
        {
            CancelEdit();
            ((Window)x).Close();
        }

        #endregion

        private void _renamer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #endregion
    }
}
