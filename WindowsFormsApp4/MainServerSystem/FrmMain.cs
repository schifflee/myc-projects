using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Timers;
using MainClientSystem;

namespace MainServerSystem
{
    public partial class FrmMain : Form
    {
        public SynchronizationContext synccontext;

        public FrmMain()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            synccontext = SynchronizationContext.Current;
        }

        ServerModule server = new ServerModule("192.168.1.124", 6000);
        System.Timers.Timer timer1;
        bool socketstatus; 
        
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                return;
            }
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                niNotifyStatus.Visible = true;
                niNotifyStatus.ShowBalloonTip(2000, Application.ProductName, "程序开始后台监听", ToolTipIcon.Info);
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ShowLog(string.Format("{0} 正在启动监听...", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            this.bgwSetNotify.RunWorkerAsync();
            this.bgwSetNotify.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwSetNotify_RunWorkerCompleted);
            Thread onlineclientsthread = new Thread(RefreshOnlineClientList)
            {
                IsBackground = true
            };
            onlineclientsthread.Start();
            this.niNotifyStatus.Text = Application.ProductName;
        }

        private void bgwSetNotify_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (socketstatus)
            {
                ShowLog(string.Format("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), server.MSG_Show));
            }
            else
            {
                ShowLog(string.Format("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), server.MSG_Error));
            }
        }

        private void RefreshOnlineClientList()
        {
            timer1 = new System.Timers.Timer(100);
            timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
            timer1.Start();
        }

        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            foreach (KeyValuePair<string, System.Net.Sockets.Socket> client in server.ClientInfos)
            {
                ListViewItem lvi = new ListViewItem
                {
                    ImageIndex = 0,
                    Text = client.Key,
                };
                lvi.SubItems.Add(server.GetClientOnlineStatus(client.Value).ToString());
                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();
        }

        private void bgwSetNotify_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Show();
            this.niNotifyStatus.Icon = this.Icon;
            socketstatus = server.InitServer();
            if (socketstatus)
            {
                niNotifyStatus.ShowBalloonTip(2000, Application.ProductName, server.MSG_Show, ToolTipIcon.Info);
            }
            else
            {
                niNotifyStatus.ShowBalloonTip(2000, Application.ProductName, server.MSG_Error, ToolTipIcon.Info);
            }
        }

        private void niNotifyStatus_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = true;
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        public void ShowLog(string str)
        {
            this.listBox1.Items.Add(str);
            this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            if (this.btnShowLog.Text == "显示消息日志")
            {
                this.btnShowLog.Text = "隐藏消息日志";
                this.Height = 618;
            }
            else
            {
                this.btnShowLog.Text = "显示消息日志";
                this.Height = 538;
            }
        }
    }
}
