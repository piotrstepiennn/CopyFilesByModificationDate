using CopyFilesByModificationDate.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Text.RegularExpressions;

namespace CopyFilesByModificationDate.ViewModels
{
    public class CopyFilesViewModel : ViewModelBase
    {
        public ObservableCollection<FileListItem> _files;
        public string SourcePathString { get; set; }
        public string DestinationPathString { get; set; }
        public IEnumerable<FileListItem> files => _files;
        public double ProgressBarValue { get; set; }

        public CopyFilesViewModel()
        {
            _files = new ObservableCollection<FileListItem>();
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
            List<FileListItem> items = new List<FileListItem>();
            if (Directory.Exists(sourcePath))
            {
                string[] fileEntries = Directory.GetFiles(sourcePath, "*.mp3");
                foreach (string file in fileEntries)
                {
                    DateTime lastModified = System.IO.File.GetLastWriteTime(file);
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if(fileName.Length > 30)
                    {
                        string shortFileName = fileName.Substring(0, 30);
                        items.Add(new FileListItem(file, lastModified, shortFileName));
                    }
                    else
                    {
                        items.Add(new FileListItem(file, lastModified, fileName));
                    }
                }                    
                
                string[] subdirectoryEntries = Directory.GetDirectories(sourcePath);
                foreach (string subdirectory in subdirectoryEntries)
                    LoadItems(subdirectory);

                _files = new ObservableCollection<FileListItem>(items.OrderBy(file => file._lastModified));
                OnPropertyChanged();
                return true;
            }
            else return false;
        }

        public bool CopyFiles(string destinationPath)
        {
            int filesCount = _files.Count + 1;
            int i = 2;
            if (Directory.Exists(destinationPath))
            {               
                foreach (var file in _files.ToList())
                {
                    ProgressBarValue = (i * 100) / filesCount;
                    OnPropertyChanged("ProgressBarValue");
                    var fileName = Path.GetFileNameWithoutExtension(file._Path);
                    var fileExtension = Path.GetExtension(file._Path);
                    var destinationFileName = destinationPath + "\\" + fileName + fileExtension;

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        bool del = DeleteItem(file);
                    });

                    if (!File.Exists(destinationFileName)) 
                    {                         
                        File.Copy(file._Path, destinationFileName);
                    }
                    i++;
                }
                return true;
            } else return false;
        }

        public bool DeleteItem(FileListItem item)
        {
            if (_files.Contains(item))
            {
                _files.Remove(item);
                OnPropertyChanged("files");
                return true;
            } 
            return false;
        }

    }
}
