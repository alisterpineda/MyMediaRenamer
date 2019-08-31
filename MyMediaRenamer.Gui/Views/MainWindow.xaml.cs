using System.Windows;
using MyMediaRenamer.Gui.ViewModels;

namespace MyMediaRenamer.Gui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MediaFilesListView_OnDrop(object sender, DragEventArgs e)
        {
            MainViewModel vm = DataContext as MainViewModel;
            string[] filePaths = e.Data.GetData(DataFormats.FileDrop) as string[];

            vm.AddMediaFiles(filePaths);
        }
    }
}
