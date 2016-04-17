using PhotoOrgWPF.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrgWPF.controller
{
    public class ImportController
    {
        protected FolderList _photoFolders;
        protected Dictionary<string, Photo> _photonameHash = new Dictionary<string, Photo>();
        protected Dictionary<string, Photo> _datetakenHash = new Dictionary<string, Photo>();
        //protected Dictionary<string, PhotoList> duplicateHash = new Dictionary<string, PhotoList>();

        public ImportController(FolderList photoFolders)
        {
            _photoFolders = photoFolders;            
        }


        public void RegisterNewPhotos(FolderList newFolders, ref PhotoList originals, ref PhotoList duplicates)
        {
            
            foreach (PhotoFolder folder in newFolders)
            {
                foreach (Photo photo in folder.PhotosDisk)
                {
                    string name = photo.Photoname;
                    string dateTakenStr = photo.GetDateTakenString();
                    if (_photonameHash.ContainsKey(name) || _datetakenHash.ContainsKey(dateTakenStr) )
                    {
                        Photo orgPhoto = null;
                        if (_photonameHash.ContainsKey(name))
                            orgPhoto = _photonameHash[name];
                        else if (_datetakenHash.ContainsKey(dateTakenStr))
                            orgPhoto = _datetakenHash[dateTakenStr];


                        originals.Add(orgPhoto);
                        duplicates.Add(photo);
                    }
                    else
                    {
                        _photonameHash.Add(name, photo);
                        _datetakenHash.Add(dateTakenStr, photo);
                    }
                }
            }
        }
    }
}
