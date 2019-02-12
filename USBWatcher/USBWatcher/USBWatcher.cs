using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevComponents.DotNetBar;


namespace USBWatcher
{
    public partial class USBWatcher : OfficeForm
    {
        public USBWatcher()
        {
            this.EnableGlass = false;
            InitializeComponent();
        }

        private delegate void THREAD_LOADINFO();
        private delegate void THREAD_REFRESHINFO();

        private void USBWatcher_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.panel1.Visible = true;
            this.expandablePanel1.Enabled = false;
            this.bgwLoad.RunWorkerAsync();
            LoadEmptytextBoxInfo();
        }

        private void LoadEmptytextBoxInfo()
        {
            this.lblDiskIndex.Text = "(空)";
            this.lblDiskType.Text = "(空)";
            this.lblPartitionIndex.Text = "(空)";
            this.lblPartitionFormat.Text = "(空)";
            this.lblTotalSpace.Text = "(空)";
            this.lblFreeSpace.Text = "(空)";
        }
      

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            try
            {
                if (m.Msg == Parameters.WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case Parameters.DBT_DEVICEARRIVAL:
                            this.bgwRefresh.RunWorkerAsync();
                            ShowBallonTip(Application.ProductName, Parameters.MSG_NEWUDISKINSERTED, ToolTipIcon.Info, 500);
                            break;
                        case Parameters.DBT_DEVICEREMOVECOMPLETE:
                            this.bgwRefresh.RunWorkerAsync();
                            ShowBallonTip(Application.ProductName, Parameters.MSG_UDISKREMOVED, ToolTipIcon.Info, 500);
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowBallonTip(string title, string text, ToolTipIcon icon, int timeout)
        {
            this.niUSBMonitor.BalloonTipTitle = title;
            this.niUSBMonitor.BalloonTipText = text;
            this.niUSBMonitor.BalloonTipIcon = ToolTipIcon.Info;
            this.niUSBMonitor.ShowBalloonTip(timeout);
        }

        public void SetNotifyIcon(NotifyIcon notify, Icon icon)
        {

        }

        private void USBWatcher_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.niUSBMonitor.Visible = true;
            }
        }

        private void niUSBMonitor_DoubleClick(object sender, EventArgs e)
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

        void RefreshInfo()
        {
            this.panel1.Visible = true;
            THREAD_REFRESHINFO loadinfo = new THREAD_REFRESHINFO(ShowInfo);
            this.BeginInvoke(loadinfo);
        }

        void LoadInfo()
        {
            this.panel1.Visible = true;          
            THREAD_LOADINFO loadinfo = new THREAD_LOADINFO(ShowInfo);
            this.BeginInvoke(loadinfo);
        }

        private void ShowInfo()
        {
            //TreeNode rootnode = null;
            //List<UDISK> udisks=GetUDISKList();
            //List<Partition> partitions;
            //this.treeView1.Enabled = true;
            //if (this.treeView1.Nodes.Count != 0)
            //{
            //    this.treeView1.Nodes.Clear();
            //}
            //foreach (UDISK udisk in udisks)
            //{
            //    rootnode = new TreeNode
            //    {
            //        Tag = udisk.DiskIndex,
            //        Text = string.Format("{0}", udisk.Caption)
            //    };
            //    int i = this.treeView1.Nodes.Add(rootnode);
            //    partitions = GetPartitionList((int)rootnode.Tag);
            //    for (int j = 0; j < partitions.Count; j++)
            //    {
            //        TreeNode childnode = new TreeNode
            //        {
            //            Tag = partitions[j].Index,
            //            Text = string.Format("{0}({1})", partitions[j].Volume, partitions[j].Letter)
            //        };
            //        this.treeView1.Nodes[i].Nodes.Add(childnode);
            //    }
            //}
            //if (GetUDISKList().Count == 0)
            //{
            //    this.treeView1.Nodes.Add("(空)");
            //    this.treeView1.Enabled = false;
            //}
        }

        private void USBWatcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.WindowState = FormWindowState.Minimized;
                e.Cancel = true;
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void ShowNormal()
        {
            this.Show();
            this.niUSBMonitor.Icon = new Icon(AppDomain.CurrentDomain.BaseDirectory + @"\icons\start.ico");
            ShowBallonTip(Application.ProductName, Parameters.MSG_MONITORING, ToolTipIcon.Info, 500);
        }

        private void bgwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowNormal();
            LoadInfo();
        }

        private void bgwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.panel1.Visible = false;
            this.expandablePanel1.Enabled = true;
        }

        private void bgwRefresh_DoWork(object sender, DoWorkEventArgs e)
        {
            RefreshInfo();
        }

        private void bgwRefresh_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.panel1.Visible = false;
            this.expandablePanel1.Enabled = true;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
            {
                LoadEmptytextBoxInfo();
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (this.treeView1.SelectedNode == null)
            //{
            //    LoadEmptytextBoxInfo();
            //}
            //else
            //{

            //    int index;
            //    UDISK udisk;
            //    int pindex;               
            //    if (this.treeView1.SelectedNode.Level == 0)
            //    {
            //        index = (int)this.treeView1.SelectedNode.Tag;
            //        udisk = GetUDISK(index);
            //        this.lblDiskIndex.Text = udisk.DiskIndex.ToString();
            //        this.lblPartitionIndex.Text = "-";
            //        this.lblDiskType.Text = udisk.DiskType;
            //        this.lblPartitionFormat.Text = "-";
            //        if (udisk.TotalSpace >= (1024 * 1024 * 1024))
            //        {
            //            this.lblTotalSpace.Text = StringUtil.ToGigaByteFormat(udisk.TotalSpace);
            //            this.lblFreeSpace.Text = "-";
            //            this.progressBarX1.Enabled = false;
            //        }
            //        else
            //        {
            //            this.lblTotalSpace.Text = StringUtil.ToMegaByteFormat(udisk.TotalSpace);
            //            this.lblFreeSpace.Text = "-";
            //            this.progressBarX1.Enabled = false;
            //        }
            //    }
            //    if (this.treeView1.SelectedNode.Level == 1)
            //    {
            //        index = (int)this.treeView1.SelectedNode.Parent.Tag;
            //        udisk = GetUDISK(index);
            //        pindex = (int)this.treeView1.SelectedNode.Tag;
            //        this.lblDiskIndex.Text = udisk.DiskIndex.ToString();
            //        this.lblPartitionIndex.Text =GetPartitionList(index)[pindex].Index.ToString();
            //        this.lblDiskType.Text = udisk.DiskType;
            //        this.lblPartitionFormat.Text = GetPartitionList(index)[pindex].Format;
            //        if (GetPartitionList(index)[pindex].TotalSpace >= (1024 * 1024 * 1024))
            //        {
            //            this.lblTotalSpace.Text = StringUtil.ToGigaByteFormat(GetPartitionList(index)[pindex].TotalSpace);
            //            this.lblFreeSpace.Text = StringUtil.ToGigaByteFormat(GetPartitionList(index)[pindex].FreeSpace);
            //            this.progressBarX1.Enabled = true;
            //            this.progressBarX1.Maximum = (int)StringUtil.ToMegaByte(GetPartitionList(index)[pindex].TotalSpace);
            //            this.progressBarX1.Value = (int)StringUtil.ToMegaByte(GetPartitionList(index)[pindex].TotalSpace - GetPartitionList(index)[pindex].FreeSpace);
            //        }
            //        else
            //        {
            //            this.lblTotalSpace.Text = StringUtil.ToMegaByteFormat(GetPartitionList(index)[pindex].TotalSpace);
            //            this.lblFreeSpace.Text = StringUtil.ToMegaByteFormat(GetPartitionList(index)[pindex].FreeSpace);
            //            this.progressBarX1.Enabled = true;
            //            this.progressBarX1.Maximum = (int)StringUtil.ToGigaByte(GetPartitionList(index)[pindex].TotalSpace);
            //            this.progressBarX1.Value = (int)StringUtil.ToGigaByte(GetPartitionList(index)[pindex].TotalSpace - GetPartitionList(index)[pindex].FreeSpace);
            //        }
            //    }
            //}
        }
    }
}
