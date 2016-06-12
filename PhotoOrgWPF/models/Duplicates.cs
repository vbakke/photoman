using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrgWPF.models
{
    class Duplicates : ObservableClass
    {

        public PhotoList OrgList { get; set; }
        public PhotoList DupList { get; set; }

        public Duplicates()
        {
            OrgList = new PhotoList();
            DupList = new PhotoList();
        }

        public void Clear()
        {
            OrgList.Clear();
            DupList.Clear();
        }
    }
}
