using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OfflineTools.Connectivity.Helpers
{
    public class ConnectivityMonitor : IDisposable, INotifyPropertyChanged
    {

        public Action Heartbeat;
        public Action StatusChange;


        void UpdateTimer()
        {
            if (null != timer)
                timer.Change(Interval, Timeout.Infinite);

        }

        private Timer timer;
        public ConnectivityMonitor()
        {
            keepAlive = true;

            timer = new Timer(timer_Elapsed, null, Timeout.Infinite, Timeout.Infinite);
            Interval = 3000;

            Test();

        }

        public ConnectivityValues Status { get; set; }
        void timer_Elapsed(object state)
        {
            if (null != Heartbeat)
                Heartbeat.DynamicInvoke();
            UpdateTimer();
        }

        bool firstGo = true;

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
                    Status = ConnectivityHelper.TestConn();
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
