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
            DataContext = _copyFilesViewModel;
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

        private async void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SourceDirectoryListView.SelectedItems.Count != 0)
            {
                var dir = (DirectoryListItem)SourceDirectoryListView.SelectedItem;
                _destinationPath = DestinationPathTextBlock.Text;
                int result = await Task.Run(() => _copyFilesViewModel.CopyFilesAsync(_destinationPath, null, null));
                if (result < 0) ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Failed to copy files!"; else ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Files succesfully copied!";
            }
        }

        private void LoadItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            _sourcePath = SourcePathTextBlock.Text;
            bool result = _copyFilesViewModel.LoadDirectories(_sourcePath);
            if (!result) ResultTextBlock.Text = "Failed to load files!"; else ResultTextBlock.Text = "Files succesfully loaded!";
        }
        
        private void DeleteSelectedItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FileListView.SelectedItems.Count != 0)
            {
                var item = FileListView.SelectedItem as FileListItem;
                bool result;
                if (item != null)
                {
                    result = _copyFilesViewModel.DeleteItem(item);
                    if (result) ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Deleted item from list";
                }
                else ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Failed to delete item";
            }
        }

        private void MoveUpBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = FileListView.SelectedItem as FileListItem;
            bool result;
            if (item != null)
            {
                result = _copyFilesViewModel.MoveItemUp(item);
                if (result) ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Moved item up";
            }
            else ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Failed to move item";
            FileListView.SelectedItem = item;
        }

        private void MoveDownBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = FileListView.SelectedItem as FileListItem;
            bool result;
            if (item != null)
            {
                result = _copyFilesViewModel.MoveItemDown(item);
                if (result) ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Moved item down";
            }
            else ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Failed to move item";
            FileListView.SelectedItem = item;
        }

        private async void CopyAllBtn_Click(object sender, RoutedEventArgs e)
        {
            _sourcePath = SourcePathTextBlock.Text;
            _destinationPath = DestinationPathTextBlock.Text;
            bool result = await Task.Run(() => _copyFilesViewModel.CopyAllFilesAsync(_sourcePath, _destinationPath));
            if (!result) ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Failed to copy files!"; else ResultTextBlock.Text = ResultTextBlock.Text + "\n" + "Files succesfully copied!";
        }

        private async void SourceDirectoryListView_SelectedAsync(object sender, RoutedEventArgs e)
        {
            if (SourceDirectoryListView.SelectedItems.Count != 0) 
            { 
                var dir = (DirectoryListItem)SourceDirectoryListView.SelectedItem;
                bool result = await _copyFilesViewModel.LoadItems(dir.FullPath);
                if (!result) ResultTextBlock.Text = "\nFailed to load files from "+ dir.DirectoryName +" directory!"; else ResultTextBlock.Text = "\nFiles succesfully loaded from " + dir.DirectoryName + " directory!";
            }
        }
    }
}
