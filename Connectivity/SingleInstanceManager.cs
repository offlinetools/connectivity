using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineTools.Connectivity
{
    public sealed class SingleInstanceManager : WindowsFormsApplicationBase
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new SingleInstanceManager().Run(args);
        }
        public SingleInstanceManager()
        {
            IsSingleInstance = true;
        }
        public App App { get; private set; }
        protected override bool OnStartup(StartupEventArgs e)
        {
            App = new App();
            App.Run();
            return false;
        }

        protected override void OnStartupNextInstance(
          StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            App.ProcessArgs(eventArgs.CommandLine.ToArray(), false);
        }

    }
}
