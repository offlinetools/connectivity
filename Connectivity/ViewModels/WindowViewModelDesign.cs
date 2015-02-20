using OfflineTools.Connectivity.Helpers;
using System;
using System.Collections.Generic;

namespace OfflineTools.Connectivity
{
    public class G
    {
        public int Interval { get; set; }

        public ConnectivityValues Status { get; set; }
    }

    public class WindowViewModelDesign
    {
        public String Test { get { return "Design"; } }

        public G ConMon
        {
            get
            {
                return new G { Status = ConnectivityValues.Checking, Interval = 1100000 };
            }
        }

        public List<HierarchicalViewModel> L
        {
            get
            {
                return new List<HierarchicalViewModel>
                {new HierarchicalViewModel
                { Text = "s10 minutes", IsEnabled = true, Data = 800000,IsSelected=true  }
                };
            }
        }
    }
}