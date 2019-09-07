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
                    Tags.Add(tag);
                }
            }
        }

        #endregion

        #region Properties

        public string FilePath => _mediaFileViewModel.FilePath;
        public List<Tag> Tags { get; } = new List<Tag>();

        #endregion
    }
}
