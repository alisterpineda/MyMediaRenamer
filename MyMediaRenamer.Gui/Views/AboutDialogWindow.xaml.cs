using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
