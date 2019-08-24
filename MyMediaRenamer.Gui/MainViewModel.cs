using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MetadataToolWpf;
using Microsoft.Win32;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;

namespace MyMediaRenamer.Gui
{
    public class MainViewModel : BaseViewModel
    {
        #region Members

        private string _pattern;
        private string _patternErrorMessage;
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

                try
                {
                    Tags = PatternParser.Parse(Pattern);
                    PatternErrorMessage = string.Empty;
                }
                catch (Exception e)
                {
                    PatternErrorMessage = e.Message;
                }

                OnPropertyChanged(nameof(Pattern));

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
            }
        }

        public List<BaseFilePathTag> Tags { get; private set; }

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
        
        #region ICommands
        public ICommand AddMediaFilesCommand { get; }
        public ICommand RemoveMediaFilesCommand { get; }
        public ICommand MoveMediaFileUpCommand { get; }
        public ICommand MoveMediaFileDownCommand { get; }
        #endregion

        #endregion

        #region Methods

        private void AddMediaFiles(string[] filePaths)
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

        #endregion

        #endregion
    }
}
