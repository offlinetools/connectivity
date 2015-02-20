using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Practices.Prism.Commands;
using OfflineTools.Connectivity.Helpers;
using OfflineTools.Connectivity.ViewModels;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;

namespace OfflineTools.Connectivity
{
    public class App : Application
    {
        private ConnectivityMonitor connectivityMonitor;
        private TaskbarIcon taskbarIcon;

        private SoundPlayer soundPlayeronhb = new SoundPlayer(Connectivity.Properties.Resources.HeartbeatOn);
        private SoundPlayer SoundPlayertransition_on = new SoundPlayer(OfflineTools.Connectivity.Properties.Resources.TransitionOn);

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("XamlResources.xaml", UriKind.Relative) });
            Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Views/NotifyIconView.xaml", UriKind.Relative) });
            connectivityMonitor = new ConnectivityMonitor();
            connectivityMonitor.Heartbeat = () => soundPlayeronhb.Play();
            connectivityMonitor.StatusChange = () => SoundPlayertransition_on.Play();
            taskbarIcon = (TaskbarIcon)FindResource("NotifyIcon");
            var intervals = GetMenu();

            notifyIconViewModel = new NotifyIconViewModel(connectivityMonitor, intervals);
            taskbarIcon.DataContext = notifyIconViewModel;
            notifyIconViewModel.ShowWindowCommand.Execute(null);
            ProcessArgs(e.Args, true);
        }

        private List<HierarchicalViewModel> GetMenu()
        {
            var intervals = new List<HierarchicalViewModel>();

            var d = new Dictionary<string, int> {{"5 seconds",5000},{"10 seconds",10000}, { "30 seconds", 30000 }
                ,{ "1 minute", 60000 },{"2 minutes",120000} };

            foreach (var l in d)
            {
                var x1 = new HierarchicalViewModel { Text = l.Key, IsEnabled = true, Data = l.Value };
                x1.IsSelected = connectivityMonitor.Interval == (int)x1.Data;
                x1.Command = new DelegateCommand(() => connectivityMonitor.Interval = (int)x1.Data);

                intervals.Add(x1);
            }

            return intervals;
        }

        public void ProcessArgs(string[] args, bool firstInstance)
        {
            if (!firstInstance)
            {
                notifyIconViewModel.ShowWindowCommand.Execute(null);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            taskbarIcon.Dispose();
            connectivityMonitor.Dispose();
            soundPlayeronhb.Dispose();
            SoundPlayertransition_on.Dispose();
        }

        private NotifyIconViewModel notifyIconViewModel;
    }
}