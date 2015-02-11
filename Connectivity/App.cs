using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Practices.Prism.Commands;
using OfflineTools.Connectivity.Helpers;
using OfflineTools.Connectivity.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;

namespace OfflineTools.Connectivity
{
    public class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            TI.Dispose();
            connectivityMonitor.Dispose();
            soundPlayeronhb.Dispose();
            SoundPlayertransition_on.Dispose();
        }
        private ConnectivityMonitor connectivityMonitor;
        public TaskbarIcon TI { get; private set; }

        SoundPlayer soundPlayeronhb = new SoundPlayer(Connectivity.Properties.Resources.HeartbeatOn );
        SoundPlayer SoundPlayertransition_on = new SoundPlayer(OfflineTools.Connectivity.Properties.Resources.TransitionOn );
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            ResourceDictionary myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source = new Uri("Views/NotifyIconView.xaml", UriKind.Relative);
            ResourceDictionary myResourceDictionary3 = new ResourceDictionary();
            myResourceDictionary3.Source = new Uri("XamlResources.xaml",UriKind.Relative );
            Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary3);
            Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
            connectivityMonitor = new ConnectivityMonitor();
            connectivityMonitor.Heartbeat = () =>
            {
                    soundPlayeronhb .Play();
            };
            connectivityMonitor.StatusChange = () =>
            {
                    SoundPlayertransition_on .Play();
            };
            TI = (TaskbarIcon)FindResource("NotifyIcon");




            var intervals=new List<HierarchicalViewModel>();

            for (var i = 1000; i <= 7000; i += 1000)
            {
                var x = new HierarchicalViewModel { Text = i.ToString(), IsEnabled = true, Data = i };
                x.IsSelected = connectivityMonitor . Interval == (int)x.Data;
                x.Command = new DelegateCommand(() => connectivityMonitor .Interval = (int)x.Data);

                intervals.Add(x);
            }



            TI.DataContext = new NotifyIconViewModel(connectivityMonitor, intervals );   
            TI.ShowBalloonTip("a", "started", BalloonIcon.Info);
            ProcessArgs(e.Args, true);
        }
        public void ProcessArgs(string[] args, bool firstInstance)
        {
            if (!firstInstance)
                TI.ShowBalloonTip("a", "new instance attempted", BalloonIcon.Info);
        }
    }
}
