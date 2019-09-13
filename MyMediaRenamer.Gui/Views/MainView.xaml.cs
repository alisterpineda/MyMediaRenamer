using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MyMediaRenamer.Gui.ViewModels;

namespace MyMediaRenamer.Gui.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void MediaFilesListView_OnDrop(object sender, DragEventArgs e)
        {
            MainViewModel vm = DataContext as MainViewModel;
            string[] filePaths = e.Data.GetData(DataFormats.FileDrop) as string[];

            vm.AddMediaFiles(filePaths);
            
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
