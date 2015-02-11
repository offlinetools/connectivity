using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using OfflineTools.Connectivity.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OfflineTools.Connectivity
{
    public class G
    {
        public int Interval { get; set; }
        public ConnectivityValues  Status{ get; set; }
    
    }
    public class WindowViewModelDesign 
    {
        public String Test { get { return "Design"; } }

        public G ConMon { get 
        {
            return new G { Status = ConnectivityValues.Connected  ,Interval=1100000};
        
        } }

        public List<HierarchicalViewModel> L
        {
            get
            {
                return  new List< HierarchicalViewModel> 
                {new HierarchicalViewModel 
                { Text = "s10 minutes", IsEnabled = true, Data = 800000,IsSelected=true  }
                };
            }
        }

   


    }
}
