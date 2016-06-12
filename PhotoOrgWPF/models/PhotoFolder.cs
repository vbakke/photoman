using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrgWPF.models
{
    public class PhotoFolder : ObservableClass
    {
        // --- Constants ---
        protected const int MAX_VIEW_COUNT = 7;
        protected const int LIMITED_VIEW_COUNT = 3;

    #region members
        // --- Memebers ---
        protected static Photo _spacerPhoto = null;
        protected string _folderName;
        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value;
                OnPropertyChanged("FolderPath");
            }
        }

        // List of Photos on the disk
        protected PhotoList _photosDisk = new PhotoList();
        public PhotoList PhotosDisk
        {
            get { return _photosDisk; }
        }
        // Abbreviated list of Photos, to be displayed
        protected PhotoList _photosView = new PhotoList();
        public PhotoList PhotosView
        {
            get { return _photosView; }
        }


        public int FileCount
        {
            get { return (_photosDisk == null) ? 0 : _photosDisk.Count; }
        }

        public bool IsNotFullView
        {
            get { return (_photosDisk == null || _photosView == null) ? true : (_photosView.Count != _photosDisk.Count); }
        }

    #endregion




        // --- Constructors ---
        public PhotoFolder()
        {
            // Set static variable if undefined
            if (_spacerPhoto == null)
                _spacerPhoto = new Photo(@"C:\Users\Vegard\Documents\src\GIT\PhotoMan\PhotoOrgWPF\images\ThreeLittleDots.jpg");
        }

        public PhotoFolder(string folderPath)
            : this()
        {

            if (folderPath!=null && folderPath.Length > 0)
                LoadFolder(folderPath);
            else
                FolderName = "";
        }
        public PhotoFolder(string folderName, PhotoList photos)
            : this()
        {
            _folderName = folderName;
            _photosDisk.Extend(photos);
            ViewShortListOnly();
        }


        // --- Public Methods ---
        public void LoadFolder(string folderPath) {
            this.FolderName = Path.GetFileName(folderPath);
            string[] paths = Directory.GetFiles(folderPath, "*.jpg");
            foreach (string path in paths)
            {
                _photosDisk.Add(new Photo(path));
            }


            ViewShortListOnly();

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

        public void ViewShortListOnly()
        {
            if (_photosDisk==null)
                return;

            if (_photosDisk.Count <= MAX_VIEW_COUNT)
                _photosDisk.CopyTo(_photosView);
            else
                _photosDisk.CopyFirstAndLastTo(_photosView, LIMITED_VIEW_COUNT, _spacerPhoto);
        }

        public bool Contains(Photo photo)
        {
            return _photosDisk.Contains(photo);
        }

        public void RemovePhoto(Photo photo)
        {
            _photosDisk.Remove(photo);
            _photosView.Remove(photo);
        }
    }




    public class PhotoList : ObservableCollection<Photo>
    {
        public PhotoList()
        {
        }
        public PhotoList(List<Photo> source)
        {
            this.Extend(source);
        }
        public PhotoList(PhotoList source)
        {
            this.Extend(source);
        }

        public void CopyTo(PhotoList target)
        {
            target.Clear();
            target.Extend(this);
        }
        public void Extend(PhotoList source)
        {
            foreach (var photo in source)
            {
                this.Add(photo);
            }
        }
        public void Extend(List<Photo> source)
        {
            foreach (var photo in source)
            {
                this.Add(photo);
            }
        }
        public void CopyFirstAndLastTo(PhotoList target, int count)
        {
            CopyFirstAndLastTo(target, count, null);
        }
        public void CopyFirstAndLastTo(PhotoList target, int count, Photo spacerPhoto)
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


    public class FolderList : ObservableCollection<PhotoFolder>
    {

    }

}
