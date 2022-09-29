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

        public FileListItems(string fileName)
        {
            _fileName = fileName;
        }
    }
}
