using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using OfflineTools.Connectivity.Helpers;
using OfflineTools.Connectivity.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OfflineTools.Connectivity.ViewModels
{
    public class NotifyIconViewModel : BindableBase
    {
        public ConnectivityMonitor ConMon { get; set; }
        public NotifyIconViewModel(ConnectivityMonitor conmon, List<HierarchicalViewModel> intervals)
        {
            this.intervals = intervals;
            ConMon = conmon;
            Win = new SettingsWindowView();
            Win.DataContext = new SettingsWindowViewModel(conmon, intervals);
            ConMon.PropertyChanged += ConMon_PropertyChanged;
            CreateMenu();
        }

        void ConMon_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            foreach (var i in intervals)
            {
                if ((int)i.Data == ConMon.Interval)
                {
                    i.IsSelected = true;
                }
                else
                    i.IsSelected = false;
            }

        }
        public SettingsWindowView Win { get; set; }
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                (() =>
                {
                    Win.Show();
                    Win.Activate();
                });
            }
        }

        public List<HierarchicalViewModel> MenuItems { get; private set; }
        List<HierarchicalViewModel> intervals;
        private void CreateMenu()
        {
            MenuItems = new List<HierarchicalViewModel> 
            { new HierarchicalViewModel 
            { Text = "Interval", Children = intervals }, new HierarchicalViewModel 
            { Text = "_Exit", IsEnabled = true, Command = ExitApplicationCommand }, new HierarchicalViewModel 
            { Text = "_About", Command = ShowWindowCommand, IsEnabled = true } };
        }
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand(() => Application.Current.Shutdown());
            }
        }

    }
}
