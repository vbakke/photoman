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
        protected const int MAX_VIEW_COUNT = 7;
        protected const int LIMITED_VIEW_COUNT = 3;

        protected static Photo _spacerPhoto = null;
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


        public int FileCount
        {
            get { return (_photosDisk == null) ? 0 : _photosDisk.Count; }
        }

        public bool IsNotFullView
        {
            get { return (_photosDisk == null || _photosView == null) ? true : (_photosView.Count != _photosDisk.Count); }
        }





        public event PropertyChangedEventHandler PropertyChanged;

        public PhotoFolder()
            : this(null) { }

        public PhotoFolder(string folderPath)
        {
            if (_spacerPhoto == null)
            {
                _spacerPhoto = new Photo(@"C:\Users\Vegard\Documents\src\GIT\PhotoMan\PhotoOrgWPF\images\ThreeLittleDots.jpg");
            }
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

            if (_photosDisk.Count <= MAX_VIEW_COUNT)
                _photosDisk.CopyTo(_photosView);
            else
                _photosDisk.CopyFirstAndLastTo(_photosView, LIMITED_VIEW_COUNT, _spacerPhoto);


            OnPropertyChanged("Photos");
            OnPropertyChanged("FileCount");
            OnPropertyChanged("IsNotFullView");
        }
        public void ViewAllPhotos()
        {
            _photosDisk.CopyTo(_photosView);
            OnPropertyChanged("Photos");
            OnPropertyChanged("IsNotFullView");
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
            CopyFirstAndLastTo(target, count, null);
        }
        public void CopyFirstAndLastTo(ObservableCollection<Photo> target, int count, Photo spacerPhoto)
        {
            target.Clear();
            for (int i = 0; i < count; i++)
            {
                target.Add(this[i]);
            }
            if (spacerPhoto != null)
                target.Add(spacerPhoto);
            for (int i = 1; i <= count; i++)
            {
                target.Add(this[ this.Count - i ]);
            }
        }
    }

}
