using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace OfflineTools.Connectivity.Helpers
{
    public class ConnectivityMonitor : IDisposable, INotifyPropertyChanged
    {
        public Action Heartbeat;
        public Action StatusChange;

        private void UpdateTimer()
        {
            if (null != timer)
                timer.Change(Interval, Timeout.Infinite);
        }

        private Timer timer;

        public void StopTesting()
        {
            keepAlive = false;
        }

        public void ResumeTesting()
        {
            keepAlive = true;
            Test();
        }

        public bool HeartbeatEnabled { get; set; }

        public bool TestsEnabled
        {
            get { return testsEnabled; }
            set
            {
                if (testsEnabled != value)
                {
                    testsEnabled = value;
                    if (!testsEnabled)
                    {
                        StopTesting();
                        Status = ConnectivityValues.Disabled;
                    }
                    else
                        ResumeTesting();

                    NotifyPropertyChanged("Enabled");
                }
            }
        }

        private bool testsEnabled;

        public ConnectivityMonitor()
        {
            HeartbeatEnabled = true;
            timer = new Timer(timer_Elapsed, null, Timeout.Infinite, Timeout.Infinite);
            TestsEnabled = true;
            Interval = 60000;
            Test();
        }

        public ConnectivityValues Status { get; set; }

        private void timer_Elapsed(object state)
        {
            if ((null != Heartbeat) && TestsEnabled && HeartbeatEnabled)
                Heartbeat.DynamicInvoke();
            UpdateTimer();
        }

        private bool firstGo = true;
        private int interval;
        private bool keepAlive;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public int Interval
        {
            get { return interval; }
            set
            {
                if (interval != value)
                {
                    interval = value;
                    UpdateTimer();
                    NotifyPropertyChanged("Interval");
                }
            }
        }

        public async void Test()
        {
            await Task.Run(() =>
            {
                ConnectivityValues lastStatus;
                while (keepAlive)
                {
                    lastStatus = Status;
                    Status = ConnectivityHelper.Test();
                    NotifyPropertyChanged("Status");
                    if (firstGo)
                    {
                        firstGo = false;
                        continue;
                    }
                    if (null != StatusChange && lastStatus != Status)
                    {
                        StatusChange();
                    }
                }
            });
        }

        #region dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                keepAlive = false;
                if (null != timer)
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                    timer.Dispose();
                }
            }
        }

        #endregion dispose

        public event PropertyChangedEventHandler PropertyChanged;
    }
}