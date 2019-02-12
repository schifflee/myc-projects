using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using log4net;
using System.Timers;

namespace USBWatcher
{
    public partial class USBWatcherHelperService : ServiceBase
    {
        public USBWatcherHelperService()
        {
            InitializeComponent();
        }

        const string processname ="USBWatcher.exe";
        Timer timer;
        Process myprocess = null;
        
        ILog log = log4net.LogManager.GetLogger("service_log");
        protected Process GetProcess(string  processname)
        {
            Process p = new Process();
            p.StartInfo.FileName = processname;           
            return p;
        }

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            if (log.IsDebugEnabled)
            {
                myprocess = GetProcess(processname);
                foreach (Process proccess in Process.GetProcesses())
                {
                    if (!proccess.ProcessName.Equals(processname))
                    {
                        try
                        {
                            myprocess = GetProcess(AppDomain.CurrentDomain.BaseDirectory + @"\USBWatcher.exe");
                            myprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            myprocess.Start();
                            log.Debug(string.Format("Program:{0} has started!", myprocess.StartInfo.FileName));
                        }
                        catch (Exception)
                        {
                            ServiceMsgBox.Show("应用程序找不到或者启动失败！", this.ServiceName);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
                
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Enabled = true;
            if (log.IsDebugEnabled)
            {
                try
                {
                    log.Debug(string.Format("service:{0} has started!", this.ServiceName));
                    log.Debug(string.Format("service:{0} begins to protect program:{1}", this.ServiceName,processname));
                }
                catch (Exception ex)
                {
                    ServiceMsgBox.Show(ex.Message, this.ServiceName);
                }
            }
        }

        protected override void OnStop()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(string.Format("service:{0} has stopped!", this.ServiceName));
            }
        }
    }
}
