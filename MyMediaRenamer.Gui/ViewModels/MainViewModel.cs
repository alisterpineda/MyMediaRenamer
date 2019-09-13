using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using MyMediaRenamer.Gui.Utilities;
using MyMediaRenamer.Gui.Utilities.WindowService;
using MyMediaRenamer.Gui.Views;

namespace MyMediaRenamer.Gui.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Members

        private string _pattern;
        private string _patternErrorMessage;
        private List<BaseTag> _tags;
        private int _selectedMediaFileIndex;
        private MediaFileViewModel _selectedMediaFileItem;

        #endregion

        #region Constructors

        public MainViewModel()
        {
            AddMediaFilesCommand = new RelayCommand(x => DoAddMediaFiles());
            RemoveMediaFilesCommand = new RelayCommand(x => DoRemoveMediaFiles(x), x => CanRemoveMediaFiles());
            MoveMediaFileUpCommand = new RelayCommand(x => DoMoveMediaFileUp(), x => CanMoveMediaFileUp());
            MoveMediaFileDownCommand = new RelayCommand(x => DoMoveMediaFileDown(), x => CanMoveMediaFileDown());
            ReloadMediaFilesCommand = new RelayCommand(x => DoReloadMediaFiles(x), x => CanReloadMediaFiles());
            StartRenamingCommand = new RelayCommand(x => DoStartRenaming(), x => CanStartRenaming());
            OpenRenamerSettingsDialogCommand = new RelayCommand(x => DoOpenRenamerSettingsDialog());
            OpenWikiOnGitHubCommand = new RelayCommand(x => DoOpenWikiOnGitHub());
            OpenAboutDialogCommand = new RelayCommand(x => DoOpenAboutDialog());
            ShutdownApplicationCommand = new RelayCommand(x => DoShutdownApplication());

        }

        #endregion

        #region Properties

        public string Pattern
        {
            get => _pattern;
            set
            {
                if (value == _pattern)
                    return;

                _pattern = value;
                OnPropertyChanged(nameof(Pattern));

                PatternErrorMessage = string.Empty;

            }
        }

        public string PatternErrorMessage
        {
            get => _patternErrorMessage;
            set
            {
                if (value == _patternErrorMessage)
                    return;

                _patternErrorMessage = value;
                OnPropertyChanged(nameof(PatternErrorMessage));
                OnPropertyChanged(nameof(HasPatternErrorMessage));
            }
        }

        public bool HasPatternErrorMessage => !string.IsNullOrEmpty(PatternErrorMessage);

        public List<BaseTag> Tags
        {
            get => _tags;
            private set
            {
                if (value == _tags)
                    return;

                _tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }

        public ObservableCollection<MediaFileViewModel> MediaFiles { get; } = new ObservableCollection<MediaFileViewModel>();

        public int SelectedMediaFileIndex
        {
            get => _selectedMediaFileIndex;
            set
            {
                if (value == _selectedMediaFileIndex)
                    return;

                _selectedMediaFileIndex = value;
                OnPropertyChanged(nameof(SelectedMediaFileIndex));
            }
        }

        public MediaFileViewModel SelectedMediaFileItem
        {
            get => _selectedMediaFileItem;
            set
            {
                if (value == _selectedMediaFileItem)
                    return;

                _selectedMediaFileItem = value;
                OnPropertyChanged(nameof(SelectedMediaFileItem));
            }
        }

        public RenamerViewModel RenamerViewModel { get; } = new RenamerViewModel();

        private RenamerSettingsDialogService RenamerSettingsDialogService { get; } = new RenamerSettingsDialogService();

        #region ICommand Properties
        public ICommand AddMediaFilesCommand { get; }
        public ICommand RemoveMediaFilesCommand { get; }
        public ICommand MoveMediaFileUpCommand { get; }
        public ICommand MoveMediaFileDownCommand { get; }
        public ICommand ReloadMediaFilesCommand { get; }
        public ICommand StartRenamingCommand { get; }
        public ICommand OpenRenamerSettingsDialogCommand { get; }
        public ICommand OpenWikiOnGitHubCommand { get; }
        public ICommand OpenAboutDialogCommand { get; }
        public ICommand ShutdownApplicationCommand { get; }
        #endregion

        #endregion

        #region Methods

        public void AddMediaFiles(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                if (MediaFiles.All(x => x.FilePath != filePath))
                    MediaFiles.Add(new MediaFileViewModel(filePath));
            }
        }

        #region ICommand Methods

        private void DoAddMediaFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Supported Media Files (*.jpeg,*.jpg,*.mov,*.mp4)|*.jpeg;*.jpg;*.mov;*.mp4",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Multiselect = true,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                AddMediaFiles(openFileDialog.FileNames);
            }
        }

        private bool CanRemoveMediaFiles()
        {
            return SelectedMediaFileIndex > -1;
        }

        private void DoRemoveMediaFiles(object param)
        {
            // Cast SelectedItemCollection to List
            List<MediaFileViewModel> mediaFilesToRemove = ((IList)param).Cast<MediaFileViewModel>().ToList();

            foreach (MediaFileViewModel mediaFile in mediaFilesToRemove)
                MediaFiles.Remove(mediaFile);
        }

        private bool CanMoveMediaFileUp()
        {
            return SelectedMediaFileIndex > 0;
        }

        private void DoMoveMediaFileUp()
        {
            MediaFiles.Move(SelectedMediaFileIndex, SelectedMediaFileIndex - 1);
        }

        private bool CanMoveMediaFileDown()
        {
            return SelectedMediaFileIndex < MediaFiles.Count - 1;
        }

        private void DoMoveMediaFileDown()
        {
            MediaFiles.Move(SelectedMediaFileIndex, SelectedMediaFileIndex + 1);
        }

        private bool CanReloadMediaFiles()
        {
            return SelectedMediaFileIndex > -1;
        }

        private void DoReloadMediaFiles(object param)
        {
            List<MediaFileViewModel> mediaFilesToReload = ((IList)param).Cast<MediaFileViewModel>().ToList();

            foreach (var mediaFile in mediaFilesToReload)
                mediaFile.Reload();
        }

        private bool CanStartRenaming()
        {
            return MediaFiles.Count > 0;
        }

        private void DoStartRenaming()
        {
            try
            {
                Tags = PatternParser.Parse(Pattern);
                if (Tags.Count == 0)
                    return;

                RenamerViewModel.ExecuteAll(MediaFiles.Where(x => x.Status == MediaFileStatus.Normal), Tags);
            }
            catch (Exception e)
            {
                PatternErrorMessage = e.Message;
            }


        }

        private void DoOpenRenamerSettingsDialog()
        {
            RenamerSettingsDialogService.ShowDialog(RenamerViewModel);
        }

        private void DoOpenWikiOnGitHub()
        {
            Process.Start(@"https://github.com/alisterpineda/MyMediaRenamer/wiki");
        }

        private void DoOpenAboutDialog()
        {
            var dlg = new AboutDialogWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            dlg.ShowDialog();
        }

        private void DoShutdownApplication()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #endregion
    }
}
