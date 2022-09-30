using CopyFilesByModificationDate.Models;
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
            DataContext = _copyFilesViewModel;
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

        private async void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = await Task.Run( () => _copyFilesViewModel.CopyFiles(_destinationPath));
            if (!result) ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Failed to copy files!"; else ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Files succesfully copied!";
        }

        private void LoadItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = _copyFilesViewModel.LoadItems(_sourcePath);
            if (!result) ResultTextBlock.Text = "Failed to load files!"; else ResultTextBlock.Text = "Files succesfully loaded!";
        }
        
        private void DeleteSelectedItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = FileListView.SelectedItem as FileListItem;
            bool result;
            if (item != null)
            {
                result = _copyFilesViewModel.DeleteItem(item);
                if (result) ResultTextBlock.Text = "Deleted item from list";
            }
            else ResultTextBlock.Text = "Failed to delete item";
            
        }
    }
}
