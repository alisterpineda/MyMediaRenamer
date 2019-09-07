using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;

namespace MyMediaRenamer.Gui.ViewModels
{
    public class RenamerViewModel : BaseViewModel
    {
        #region Members

        private Renamer _renamer = new Renamer();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void Execute(IEnumerable<MediaFileViewModel> mediaFiles, IEnumerable<BaseTag> Tags)
        {
            _renamer.Execute(mediaFiles.Select(x => x.MediaFile), Tags);
        }

        #endregion
    }
}
