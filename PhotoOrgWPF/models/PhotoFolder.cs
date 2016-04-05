using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrgWPF.models
{
    class PhotoFolder : INotifyPropertyChanged
    {
        protected string _folderName;
        public string FolderPath
        {
            get { return _folderName; }
            set
            {
                _folderName = value;
                OnPropertyChanged("FolderPath");
            }
        }

        public int FileCount
        {
            get { return (Photos==null) ? 0 : Photos.Count; }
        }

        protected PhotoList _photos = new PhotoList();
        public PhotoList Photos
        {
            get { return _photos; }
            set
            {
                _photos = value;
                OnPropertyChanged("Photos");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PhotoFolder()
            : this(null) { }

        public PhotoFolder(string folderPath)
        {
            this.Photos.Clear();

            if (folderPath!=null && folderPath.Length > 0)
                LoadFolder(folderPath);
            else
                FolderPath = "";
        }

        public void LoadFolder(string folderPath) {

            this.FolderPath = folderPath;
            string[] paths = Directory.GetFiles(folderPath, "*.jpg");
            foreach (string path in paths)
            {
                Photos.Add(new Photo(path));
            }
            OnPropertyChanged("Photos");
            OnPropertyChanged("FileCount");

        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    class PhotoList : ObservableCollection<Photo>
    {
        public PhotoList()
        {
        }
    }

}
