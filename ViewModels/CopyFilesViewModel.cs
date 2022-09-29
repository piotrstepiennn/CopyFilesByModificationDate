using CopyFilesByModificationDate.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace CopyFilesByModificationDate.ViewModels
{
    public class CopyFilesViewModel : ViewModelBase
    {
        private readonly ObservableCollection<FileListItems> _files;
        public ICommand CopyCommand { get; }
        public ICommand SourcePathCommand { get; }
        public ICommand DestinationPathCommand { get; }
        public string SourcePathString { get; set; }
        public string DestinationPathString { get; set; }
        public IEnumerable<FileListItems> files => _files;
        public FileListItems FileListItems { get; }
        public CopyFilesViewModel()
        {
            _files = new ObservableCollection<FileListItems>();
            _files.Add(new FileListItems("test"));
        }
        public string GetPath()
        {
            string folderPath="";
            FolderBrowserDialog filePath = new FolderBrowserDialog();
            if (filePath.ShowDialog() == DialogResult.OK)
            {
                folderPath = filePath.SelectedPath;
            }
            return folderPath;
        }
        
        public bool LoadItems(string path)
        {
            return false;
        }

    }
}
