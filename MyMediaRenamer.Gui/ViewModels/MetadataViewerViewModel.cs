using System.Collections.Generic;
using MetadataExtractor;

namespace MyMediaRenamer.Gui.ViewModels
{
    public class MetadataViewerViewModel : BaseViewModel
    {
        #region Members

        private MediaFileViewModel _mediaFileViewModel;
        #endregion

        #region Constructors

        public MetadataViewerViewModel(MediaFileViewModel mediaFileViewModel)
        {
            _mediaFileViewModel = mediaFileViewModel;
            foreach (var directory in mediaFileViewModel.MetadataDirectories)
            {
                foreach (var tag in directory.Tags)
                {
                    Tags.Add(new TagViewModel(directory, tag));
                }
            }
        }

        #endregion

        #region Properties

        public string FilePath => _mediaFileViewModel.FilePath;
        public List<TagViewModel> Tags { get; } = new List<TagViewModel>();

        #endregion
    }
}
