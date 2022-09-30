using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFilesByModificationDate.Models
{
    public class FileListItems
    {
        public string _fileName { get; set; }
        public string _Path { get; set; }
        public DateTime _lastModified { get; set; }

        public FileListItems(string Path, DateTime LastModified, string fileName)
        {
            _Path = Path;
            _lastModified = LastModified;
            _fileName = fileName;
        }
        public FileListItems() { }
    }
}
