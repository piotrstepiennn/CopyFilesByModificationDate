using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFilesByModificationDate.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase mainViewModel { get; }

        public MainViewModel()
        {
            mainViewModel = new CopyFilesViewModel();
        }
    }
}
