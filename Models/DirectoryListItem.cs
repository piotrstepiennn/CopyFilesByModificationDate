using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFilesByModificationDate.Models
{
    public class DirectoryListItem
    {
        public string DirectoryName { get; set; }
        public string FullPath { get; set; }

        public DirectoryListItem() { }

        public DirectoryListItem(string directoryName, string fullPath)
        {
            DirectoryName = directoryName;
            FullPath = fullPath;
        }
    }
}
