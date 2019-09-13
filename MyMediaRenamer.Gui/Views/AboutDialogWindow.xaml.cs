using System.Diagnostics;
using System.Windows;

namespace MyMediaRenamer.Gui.Views
{
    /// <summary>
    /// Interaction logic for AboutDialogWindow.xaml
    /// </summary>
    public partial class AboutDialogWindow : Window
    {
        public AboutDialogWindow()
        {
            InitializeComponent();
        }

        private void GitHubButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(@"https://github.com/alisterpineda/MyMediaRenamer");
        }
    }
}
