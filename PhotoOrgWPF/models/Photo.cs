using System;
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
    class Photo : INotifyPropertyChanged
    {
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


        public event PropertyChangedEventHandler PropertyChanged;

        private static Regex re = new Regex(":");

        public Photo(string fullPath)
        {
            this.FullPath = fullPath;
            this.Photoname = Path.GetFileNameWithoutExtension(fullPath);
            this.DateTaken = GetDateTakenFromImage(fullPath);
            
        }

        public static DateTime GetDateTakenFromImage(string path)
        {
            if (path == null || path == "")
            {
                return DateTime.MinValue;
            }
            else
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (Image myImage = Image.FromStream(fs, false, false))
                    {
                        PropertyItem propItem = myImage.GetPropertyItem(0x9003);
                        string dateTaken = re.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        return DateTime.Parse(dateTaken);
                    }
                }
            }
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
}
