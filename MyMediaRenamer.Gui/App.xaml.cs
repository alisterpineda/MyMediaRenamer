using System.Windows;
using MyMediaRenamer.Gui.ViewModels;
using MyMediaRenamer.Gui.Views;

namespace MyMediaRenamer.Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            CommonWindow mainWindow = new CommonWindow
            {
                Title = "MyMediaRenamer",
                Height = 800,
                MinHeight = 400,
                Width = 450,
                MinWidth = 300,
                DataContext = new MainViewModel()
            };
            mainWindow.Show();
        }
    }
}
