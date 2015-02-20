using Microsoft.Practices.Prism.Mvvm;
using OfflineTools.Connectivity.Helpers;
using System;
using System.Collections.Generic;

namespace OfflineTools.Connectivity.ViewModels
{
    public class SettingsWindowViewModel : BindableBase
    {
        public ConnectivityMonitor ConMon { get; set; }

        public String Test { get { return "Run"; } }

        public SettingsWindowViewModel(ConnectivityMonitor conmon, List<HierarchicalViewModel> intervals)
        {
            this.intervals = intervals;
            ConMon = conmon;
        }

        public List<HierarchicalViewModel> L { get { return intervals; } }

        private List<HierarchicalViewModel> intervals;
    }
}