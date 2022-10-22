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
using System.Windows.Shapes;
using Path = System.IO.Path;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace CopyFilesByModificationDate.ViewModels
{
    public class CopyFilesViewModel : ViewModelBase
    {
        public ObservableCollection<FileListItem> _files;
        public ObservableCollection<DirectoryListItem> _SourceDirectories;
        public ObservableCollection<DirectoryListItem> _DestinationDirectories;
        public string SourcePathString { get; set; }
        public string DestinationPathString { get; set; }
        public IEnumerable<FileListItem> files => _files;
        public IEnumerable<DirectoryListItem> SourceDirectories => _SourceDirectories;
        public IEnumerable<DirectoryListItem> DestinationDirectories => _DestinationDirectories;
        public double ProgressBarValue { get; set; }

        public CopyFilesViewModel()
        {
            _SourceDirectories = new ObservableCollection<DirectoryListItem>();
            _DestinationDirectories = new ObservableCollection<DirectoryListItem>();
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
        
        public bool LoadDirectories(string sourcePath)
        {
            try
            {
                _SourceDirectories.Clear();
                string[] dirs = Directory.GetDirectories(sourcePath, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    string DirectoryName = Path.GetFileName(dir);
                    _SourceDirectories.Add(new DirectoryListItem(DirectoryName, dir));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LoadItems(string sourcePath)
        {
            List<FileListItem> items = new List<FileListItem>();
            if (Directory.Exists(sourcePath))
            {
                string[] fileEntries = Directory.GetFiles(sourcePath, "*.mp3");
                foreach (string file in fileEntries)
                {
                    DateTime lastModified = System.IO.File.GetLastWriteTime(file);
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if (fileName.Length > 30)
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


        public async Task<int> CopyFilesAsync(string destinationPath, int? count, int? alternativeProgressBarCounter)
        {
            try
            {
                int filesCount = _files.Count;
                int i = 1;
                if (Directory.Exists(destinationPath))
                {
                    foreach (var file in _files.ToList())
                    {
                        if (count.HasValue && alternativeProgressBarCounter.HasValue)
                        {
                            ProgressBarValue = (double)((alternativeProgressBarCounter * 100) / count);
                            alternativeProgressBarCounter++;
                        }
                        else
                        {
                            ProgressBarValue = (i * 100) / filesCount;
                            i++;
                        }

                        OnPropertyChanged("ProgressBarValue");
                        var fileName = Path.GetFileNameWithoutExtension(file._Path);
                        var fileExtension = Path.GetExtension(file._Path);
                        var DirectoryName = destinationPath + "\\" + Path.GetFileName(Path.GetDirectoryName(file._Path));
                        Directory.CreateDirectory(DirectoryName);
                        var destinationFile = DirectoryName + "\\" + fileName + fileExtension;

                        var FolderName = Path.GetFileName(DirectoryName);
                        var folder = new DirectoryListItem(FolderName, destinationPath);
                        if (!_DestinationDirectories.Any(x => x.DirectoryName == FolderName))
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                _DestinationDirectories.Add(folder);
                            });
                        }

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            bool del = DeleteItem(file);
                        });

                        if (!File.Exists(destinationFile))
                        {
                            await Task.Run(() => File.Copy(file._Path, destinationFile));
                        }
                        
                    }
                    if (count.HasValue && alternativeProgressBarCounter.HasValue)
                    {
                        return (int)alternativeProgressBarCounter;
                    }
                    else
                    {
                        return 1;
                    }
                        
                }
                else return -1;
            }
            catch
            {
                return -1;
            }
            
        }

        public async Task<bool> CopyAllFilesAsync(string sourcePath, string destinationPath)
        {
            try
            {
                
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    _DestinationDirectories.Clear();
                });
                int count = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories).Length;
                int result = 1;
                foreach (DirectoryListItem dir in SourceDirectories)
                {
                    string newDestinationPath = destinationPath + "\\"+dir.DirectoryName;
                    Directory.CreateDirectory(newDestinationPath);
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _DestinationDirectories.Add(new DirectoryListItem(dir.DirectoryName, destinationPath));
                    });

                    await LoadItems(dir.FullPath);
                    result += await Task.Run(() => CopyFilesAsync(destinationPath, count, result));               
                }
                return true;
            } 
            catch { return false; }
            
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

        public bool MoveItemUp(FileListItem item)
        {
            try
            {
                if (_files.Contains(item))
                {
                    int index = _files.IndexOf(item);
                    FileListItem tmp = new FileListItem();

                    tmp = _files[index - 1];
                    _files[index - 1] = item;
                    _files[index] = tmp;
                    OnPropertyChanged("files");
                    return true;
                }
                return false;
            } catch { return false; }

        }

        public bool MoveItemDown(FileListItem item)
        {
            try
            {
                if (_files.Contains(item))
                {
                    int index = _files.IndexOf(item);
                    FileListItem tmp = new FileListItem();

                    tmp = _files[index + 1];
                    _files[index + 1] = item;
                    _files[index] = tmp;
                    OnPropertyChanged("files");
                    return true;
                }
                return false;
            } catch { return false; }

        }

    }
}
