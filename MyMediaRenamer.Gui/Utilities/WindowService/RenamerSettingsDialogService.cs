using System;
using System.Windows;
using MyMediaRenamer.Gui.ViewModels;
using MyMediaRenamer.Gui.Views;

namespace MyMediaRenamer.Gui.Utilities.WindowService
{
    public class RenamerSettingsDialogService : IDialogService
    {
        private CommonWindow _renamerSettingsDialog;

        public bool? ShowDialog(object dataContext)
        {
            if (!(dataContext is RenamerViewModel renamerViewModel))
                throw new InvalidOperationException();

            renamerViewModel.BeginEdit();

            _renamerSettingsDialog = new CommonWindow
            {
                Title = "Renamer Settings",
                Owner = Application.Current.MainWindow,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                DataContext = renamerViewModel
            };

            return _renamerSettingsDialog.ShowDialog();
        }
    }
}
