﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhotoOrgWPF.models
{
    public class Photo : ObservableClass
    {
        // --- Members ---
        // FullPath
        protected string _fullPath;
        public string FullPath
        {
            get { return _fullPath; }
            set
            {
                _fullPath = value;
                OnPropertyChanged("FullPath");
            }
        }

        // Filename
        protected string _photoname;
        public string Photoname
        {
            get { return _photoname; }
            set
            {
                _photoname = value;
                OnPropertyChanged("Photoname");
            }
        }

        // DateTaken
        protected DateTime _dateTaken;
        public DateTime DateTaken
        {
            get { return _dateTaken; }
            set
            {
                _dateTaken = value;
                OnPropertyChanged("DateTaken");
            }
        }



        private static Regex re = new Regex(":");

        // --- Constructors ---
        public Photo(string fullPath)
        {
            this.FullPath = fullPath;
            this.Photoname = Path.GetFileNameWithoutExtension(fullPath);
            this.DateTaken = GetDateTakenFromImage(fullPath);
            
        }

        // --- Public methods ---
        public string GetDateTakenString()
        {
            return Photo.GetDateTakenStringFromImage(this.FullPath);
        }
        


        public static DateTime GetDateTakenFromImage(string path)
        {
            string dateString = Photo.GetDateTakenStringFromImage(path);
            if (dateString != null && dateString != "")
            {
                string dateTaken = re.Replace(dateString, "-", 2);
                return DateTime.Parse(dateTaken);
            }
            return DateTime.MinValue;
        }

        public static string GetDateTakenStringFromImage(string path)
        {
            if (path == null || path == "")
            {
                return "";
            }
            else
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (Image myImage = Image.FromStream(fs, false, false))
                    {
                        PropertyItem propItem = null;
                        try
                        {
                            propItem = myImage.GetPropertyItem(0x9003);
                        }
                        catch (ArgumentException)
                        {
                            try
                            {
                                propItem = myImage.GetPropertyItem(306);
                            }
                            catch (ArgumentException)
                            {
                                // pass
                            }
                        }

                        if (propItem != null)
                        {
                            return Encoding.UTF8.GetString(propItem.Value);
                        }
                        else
                        {
                            return File.GetLastWriteTime(path).ToString("yyyy:MM:dd HH:mm:ss");
                        }
                    }
                }
            }
        }


    }
}
