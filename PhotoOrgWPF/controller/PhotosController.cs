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
        protected FolderList _photoFolders;
        protected SourceFolder _sourceFolder;



        public PhotosController(FolderList photoFolders)
        {
            _photoFolders = photoFolders;            
        }


        public void LoadFolder(SourceFolder sourceFolder)
        {
            this._sourceFolder = sourceFolder;
            //_photoFolders.Clear();
            string srcPath = _sourceFolder.Path;
            _photoFolders.Add(new PhotoFolder(srcPath));
                    
        }

        public void AutoSplitFolders()
        {
            int i = 0;
            while (i < _photoFolders.Count)
            {
                int newFolders = AutoSplitFolder(i);
                i += newFolders;
            }
        }

        protected int AutoSplitFolder(int index)
        {
            PhotoFolder mainFolder = _photoFolders[index];
            _photoFolders.RemoveAt(index);


            Dictionary<DateTime, List<Photo>> photos = new Dictionary<DateTime, List<Photo>>();
            DateTime date;

            // Identify dates for photos
            Debug.WriteLine("=== Splitting " + mainFolder.FileCount + " photos for " + _sourceFolder.DisplayName + " ===");
            foreach (var photo in mainFolder.PhotosDisk)
            {
                date = photo.DateTaken.Date;
                if (!photos.ContainsKey(date))
                    photos[date] = new List<Photo>();
                photos[date].Add(photo);
            }

            // Split into subfolders
            PhotoFolder newFolder;
            List<PhotoFolder> newFolderList = new List<PhotoFolder>();
            List<DateTime> keys = photos.Keys.ToList();
            keys.Sort();
            //* -- Debug --
            for (int i=0; i<keys.Count; i++)
                Debug.WriteLine("- #" + i + " Date " + keys[i].ToShortDateString() + ": " + photos[keys[i]].Count);
            // */

            int newFolderCount = 0;
            int start = 0;
            for (int end = 0; end < keys.Count; end++)
            {
                date = keys[end];
                if (photos[date].Count >= MIN_LIMIT_AUTOSPLIT)
                {
                    // Store photos up til this date, in one folder
                    if (end > start)
                    {
                        newFolder = CreateSubfolder(photos, keys, start, end);
                        _photoFolders.Insert(index + newFolderCount, newFolder);
                        newFolderCount++;
                        //newFolderList.Add(newFolder);
                    }
                    // Store big folder in its own folder
                    //Debug.WriteLine("Storing " + keys[end].ToShortDateString() + " with " + photos[keys[end]].Count + " photos on its own");
                    newFolder = CreateSubfolder(photos, keys, end, end+1);
                    _photoFolders.Insert(index + newFolderCount, newFolder);
                    newFolderCount++;
                    //newFolderList.Add(newFolder);

                    // Prepare next round
                    start = end + 1;
                }
            }
            // Store remaining photos in last folder
            if (start < keys.Count)
            {
                newFolder = CreateSubfolder(photos, keys, start, keys.Count);
                _photoFolders.Insert(index + newFolderCount, newFolder);
                newFolderCount++;
                //newFolderList.Add(newFolder);
            }
            Debug.WriteLine("Split folder in " + newFolderCount + " dates");
            for (int i=0; i<newFolderList.Count; i++) {
                
            }

            return newFolderCount;
        }

        protected PhotoFolder CreateSubfolder(Dictionary<DateTime, List<Photo>> photos, List<DateTime> keys, int start, int end)
        {

            PhotoFolder newFolder = new PhotoFolder();
            PhotoList photoList = new PhotoList();

            // Join photos into one list
            for (int i = start; i < end; i++)
            {
                //Debug.WriteLine("Extending "+keys[i].ToShortDateString()+" with "+photos[keys[i]].Count+" photos");
                photoList.Extend(photos[keys[i]]);
            }

            // Create folder name
            string foldername = ((DateTime)keys[end-1]).ToShortDateString();
            if (start+1 < end-1)
                foldername = ((DateTime)keys[start]).ToShortDateString() + " - " + foldername;


            Debug.WriteLine("Creating '" + foldername + "' with " + photoList.Count + " photos");
            newFolder = new PhotoFolder(foldername, photoList);

            return newFolder;
        }

    }
}
