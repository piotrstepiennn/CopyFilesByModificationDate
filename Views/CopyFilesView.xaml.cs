using CopyFilesByModificationDate.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CopyFilesByModificationDate.Views
{
    /// <summary>
    /// Interaction logic for CopyFilesView.xaml
    /// </summary>
    public partial class CopyFilesView : UserControl
    {
        CopyFilesViewModel _copyFilesViewModel;
        private string _sourcePath;
        private string _destinationPath;
       
        public CopyFilesView()
        {
            _copyFilesViewModel = new CopyFilesViewModel();
            InitializeComponent();
        }

        private void SourcePathBtn_Click(object sender, RoutedEventArgs e)
        {
            _sourcePath = _copyFilesViewModel.GetPath();
            SourcePathTextBlock.Text = _sourcePath;
        }

        private void DestPathBtn_Click(object sender, RoutedEventArgs e)
        {
            _destinationPath = _copyFilesViewModel.GetPath();
            DestinationPathTextBlock.Text = _destinationPath;
        }

        private void CopyBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = _copyFilesViewModel.LoadItems(_sourcePath);
            if (!result) ResultTextBlock.Text = "Failed to load files!";
        }
    }
}
