using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrgWPF.models
{

    public class SourceFolder : ObservableClass
    {
        // --- Members ---
        protected string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }


        protected string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }

        protected SourceFolder _target;
        public SourceFolder Target
        {
            get { return _target; }
            set
            {
                _target = value;
                OnPropertyChanged("Target");
            }
        }



        // --- Constructors ---
        public SourceFolder(string displayName, string path)
        {
            _displayName = displayName;
            _path = path;
            _target = null;
        }
        public SourceFolder(string displayName, string path, SourceFolder target)
        {
            _displayName = displayName;
            _path = path;
            _target = target;
        }

    }
}
