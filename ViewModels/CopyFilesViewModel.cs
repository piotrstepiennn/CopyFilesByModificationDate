using CopyFilesByModificationDate.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        public ObservableCollection<FileListItems> _files;
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
        
        public bool LoadItems(string sourcePath)
        {
            if (Directory.Exists(sourcePath))
            {
                string[] fileEntries = Directory.GetFiles(sourcePath, "*.mp3");
                foreach (string file in fileEntries)
                {
                    DateTime lastModified = System.IO.File.GetLastWriteTime(file);
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    _files.Add(new FileListItems(file, lastModified, fileName));
                }                    
                
                string[] subdirectoryEntries = Directory.GetDirectories(sourcePath);
                foreach (string subdirectory in subdirectoryEntries)
                    LoadItems(subdirectory);
                _files = new ObservableCollection<FileListItems>(_files.OrderBy(file => file._lastModified));
                return true;
            }
            else return false;
        }

        public bool CopyFiles(string destinationPath)
        {
            if (Directory.Exists(destinationPath))
            {               
                foreach (var file in _files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file._Path);
                    var fileExtension = Path.GetExtension(file._Path);
                    var destinationFileName = destinationPath + "\\" + fileName + fileExtension;
                    if (!File.Exists(destinationFileName)) 
                    {                         
                        File.Copy(file._Path, destinationFileName);
                    }
                }
                return true;
            } else return false;
        }

    }
}
