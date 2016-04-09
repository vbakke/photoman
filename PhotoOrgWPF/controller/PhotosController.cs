using PhotoOrgWPF.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrgWPF.controller
{
    public class PhotosController
    {
        public const int MIN_LIMIT_AUTOSPLIT = 5;
        protected ObservableCollection<PhotoFolder> _photoFolders;
        protected SourceFolder _sourceFolder;



        public PhotosController(ObservableCollection<PhotoFolder> photoFolders)
        {
            _photoFolders = photoFolders;            
        }


        public void LoadFolder(SourceFolder sourceFolder)
        {
            this._sourceFolder = sourceFolder;
            _photoFolders.Clear();
            string srcPath = _sourceFolder.Path;
            _photoFolders.Add(new PhotoFolder(srcPath));
                    
        }

        public void AutoSplitFolders()
        {
            for (int i = 0; i < _photoFolders.Count; i++)
            {
                AutoSplitFolder(i);
            }
        }

        protected void AutoSplitFolder(int index)
        {
            PhotoFolder mainFolder = _photoFolders[index];
            Dictionary<DateTime, List<Photo>> photos = new Dictionary<DateTime, List<Photo>>();
            DateTime date;

            // Identify dates for photos
            Debug.WriteLine("=== Splitting " + mainFolder.FileCount + " photos for " + _sourceFolder.DisplayName + " ===");
            foreach (var photo in mainFolder.PhotosDisk)
            {
                date = photo.DateTaken.Date;
                if (!photos.ContainsKey(date))
                {
                    //Debug.WriteLine("Creating Hash key: " + date.ToShortDateString());
                    photos[date] = new List<Photo>();
                }
                photos[date].Add(photo);
                //Debug.WriteLine("Added '"+photo.Photoname+"' to "+date.ToString("dd.MM.yyyy")+", "+photos[date].Count+" photos");

            }

            // Split into subfolders
            PhotoList photoList;
            PhotoFolder newFolder;
            List<PhotoFolder> newFolderList = new List<PhotoFolder>();
            var keys = photos.Keys.ToList();
            keys.Sort();
            // Debug
            foreach (DateTime tmpDate in keys)
                Debug.WriteLine("- Date " + tmpDate.ToShortDateString() + ": " + photos[tmpDate].Count);

            
            int start = -1;
            string subfolder;
            for (int end = 0; end < keys.Count; end++)
            {
                date = keys[end];
                Debug.WriteLine(" - Date " + date.ToShortDateString() + ": " + photos[date].Count);
                if (photos[date].Count >= MIN_LIMIT_AUTOSPLIT)
                {
                    // Store photoes up til this date, in one folder
                    if (end > 0)
                    {
                        photoList = new PhotoList();
                        for (int i = start + 1; i < end; i++)
                        {
                            Debug.WriteLine("Extending "+date.ToShortDateString()+" with "+photos[keys[i]].Count+" photos");
                            photoList.Extend(photos[keys[i]]);
                        }
                        subfolder = ((DateTime)keys[end-1]).ToShortDateString();
                        if (start < 0)
                            start = 0;
                        if (start+1 < end-1)
                            subfolder = ((DateTime)keys[start]).ToShortDateString() + " - " + subfolder;
                        newFolder = new PhotoFolder(subfolder, photoList);
                        newFolderList.Add(newFolder);
                    }
                    // Store big folder in its own folder
                    Debug.WriteLine("Storing " + keys[end].ToShortDateString() + " with " + photos[keys[end]].Count + " photos on its own");
                    subfolder = date.ToShortDateString();
                    photoList = new PhotoList(photos[keys[end]]);
                    newFolder = new PhotoFolder(subfolder, photoList); 
                    newFolderList.Add(newFolder);
                    // Prepare next round
                    start = end;
                }
            }

            Debug.WriteLine("Split folder in " + newFolderList.Count + " dates");
            _photoFolders.RemoveAt(index);
            for (int i=0; i<newFolderList.Count; i++) {
                _photoFolders.Insert(index+i, newFolderList[i]);
            }
            

        }

    }
}
