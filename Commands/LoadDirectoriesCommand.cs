using CopyFilesByModificationDate.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFilesByModificationDate.Commands
{
    public class LoadDirectoriesCommand : CommandBase
    {
        private string _sourcePath;
        private ObservableCollection<DirectoryListItem> SourceDirectories;
        public LoadDirectoriesCommand(string SourcePath, ObservableCollection<DirectoryListItem> _SourceDirectories)
        {
            _sourcePath = SourcePath;
            SourceDirectories = _SourceDirectories;
        }
        public override void Execute(object? parameter)
        {
            SourceDirectories.Clear();
            string[] dirs = Directory.GetDirectories(_sourcePath, "*", SearchOption.TopDirectoryOnly);
            foreach (string dir in dirs)
            {
                string DirectoryName = Path.GetFileName(dir);
                SourceDirectories.Add(new DirectoryListItem(DirectoryName, dir));
            }
        }
    }
}
