using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MetadataExtractor;
using MyMediaRenamer.Gui.Utilities;

namespace MyMediaRenamer.Gui.ViewModels
{
    public class TagViewModel : BaseViewModel
    {
        #region Members

        private readonly Directory _directory;
        private readonly Tag _tag;

        #endregion

        #region Constructors

        public TagViewModel(Directory directory, Tag tag)
        {
            _directory = directory;
            _tag = tag;

            CopyTagNameToClipboardCommand = new RelayCommand(x => DoCopyTagNameToClipboard());
            CopyValueToClipboardCommand = new RelayCommand(x => DoCopyValueToClipboard());
            CopyValueRawToClipboardCommand = new RelayCommand(x => DoCopyValueRawToClipboard());
        }

        #endregion

        #region Properties

        public string Name => _tag.Name;
        public string DirectoryName => _directory.Name;
        public string Value => _tag.Description;
        public string ValueRaw => _directory.GetString(_tag.Type);

        #region ICommand Properties
        public ICommand CopyTagNameToClipboardCommand { get; }
        public ICommand CopyValueToClipboardCommand { get; }
        public ICommand CopyValueRawToClipboardCommand { get; }
        #endregion

        #endregion

        #region Methods

        #region ICommand Methods

        private void DoCopyTagNameToClipboard()
        {
            Clipboard.SetText(Name);
        }

        private void DoCopyValueToClipboard()
        {
            Clipboard.SetText(Value);
        }

        private void DoCopyValueRawToClipboard()
        {
            Clipboard.SetText(ValueRaw);
        }

        #endregion

        #endregion
    }
}
