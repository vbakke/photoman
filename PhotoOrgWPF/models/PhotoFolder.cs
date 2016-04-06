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
        protected const int MaxViewCount = 7;
        protected const int CachedViewCount = 3;

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
            get { return (_photosDisk == null) ? 0 : _photosDisk.Count; }
        }

        protected PhotoList _photosDisk = new PhotoList();
        protected PhotoList _photosView = new PhotoList();
        public PhotoList Photos
        {
            get { return _photosView; }
            set
            {
                _photosView = value;
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
                _photosDisk.Add(new Photo(path));
            }

            if (_photosDisk.Count <= MaxViewCount)
                _photosDisk.CopyTo(_photosView);
            else
                _photosDisk.CopyFirstAndLastTo(_photosView, CachedViewCount);


            OnPropertyChanged("Photos");
            OnPropertyChanged("FileCount");
        }
        public void ViewAllPhotos()
        {
            _photosDisk.CopyTo(_photosView);
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
        public void CopyTo(ObservableCollection<Photo> target)
        {
            target.Clear();
            foreach (var photo in this)
            {
                target.Add(photo);
            }            
        }
        public void CopyFirstAndLastTo(ObservableCollection<Photo> target, int count)
        {
            target.Clear();
            for (int i = 0; i < count; i++)
            {
                target.Add(this[i]);
            }
            //target.Add(new Photo(""));
            for (int i = 1; i <= count; i++)
            {
                target.Add(this[ this.Count - i ]);
            }
        }
    }

}
