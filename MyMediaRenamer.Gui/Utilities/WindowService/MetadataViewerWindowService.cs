using System;
using MyMediaRenamer.Gui.ViewModels;
using MyMediaRenamer.Gui.Views;

namespace MyMediaRenamer.Gui.Utilities.WindowService
{
    public class MetadataViewerWindowService : IWindowService
    {
        private CommonWindow _metadataViewerWindow;

        public void Show(object dataContext)
        {
            if (!(dataContext is MetadataViewerViewModel metadataViewerViewModel))
                throw new InvalidOperationException();

            _metadataViewerWindow = new CommonWindow
            {
                Title = "Metadata Viewer ('" + metadataViewerViewModel.FilePath + "')",
                Height = 600,
                MinHeight = 300,
                Width = 450,
                MinWidth = 225,
                DataContext = dataContext
            };

            _metadataViewerWindow.Show();
        }
    }
}
