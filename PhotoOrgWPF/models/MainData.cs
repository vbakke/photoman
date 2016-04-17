using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrgWPF.models
{
    class MainData
    {
        public FolderList photoFolders = new FolderList();
        public ObservableCollection<SourceFolder> sourceFolders = new ObservableCollection<SourceFolder>();

        public Duplicates duplicates = new Duplicates();
    }
}
