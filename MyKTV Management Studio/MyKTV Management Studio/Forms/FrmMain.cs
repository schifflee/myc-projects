using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using DevComponents.DotNetBar;
using System.Timers;

namespace MyKTV_Management_Studio
{
    public partial class FrmMain : OfficeForm
    {
        public FrmMain()
        {
            InitializeComponent();
            this.EnableGlass = false;
            CheckForIllegalCrossThreadCalls = false;
        }

        IAdministratorService adminservice = new AdministratorServiceImpl();
        System.Timers.Timer timer = new System.Timers.Timer();

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.tabMain.Visible = false;
            this.tabMain.Tabs.Clear();      
            this.mlblAdminName.Text = PARAMS.AdminName;
            this.tlblProgramStatus.Text = string.Format("数据库更新时间：{0}", new SongServiceImpl().GetDBUpdateDate().ToShortDateString());
            this.tbiDateTime.Text = string.Format("{0}", DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm:ss"));
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Interval = 1000;            
            timer.Enabled = true; 
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.tbiDateTime.Text = string.Format("{0}", DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm:ss"));
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (adminservice.SetAdminSatusByAdminName(PARAMS.AdminName, 0))
            {
                MessageBoxEx.EnableGlass = false;
                DialogResult dr = MessageBoxEx.Show(PARAMS.MSG_WINDOWCLOSINGCONFIRM, PARAMS.TITLE_WINDOWCLOSINGCONFIRM, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (dr)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        Application.ExitThread();
                        e.Cancel = false;
                        break;
                    case DialogResult.No:
                        Application.ExitThread();
                        Process.Start(Assembly.GetExecutingAssembly().Location);
                        break;
                }
            }
            else
            {
                MessageBox.Show(PARAMS.ERRORMSG_LOGOUTERROR, PARAMS.TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbiFindSingers_Click(object sender, EventArgs e)
        {

        }

        private void tbiLogout_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            Process.Start(Assembly.GetExecutingAssembly().Location);
        }

        private void tbiFindSongs_Click(object sender, EventArgs e)
        {
            this.tabMain.Visible = true;
            SetMdiForm("歌曲信息","FrmSongsInfo" );
        }

        public void SetMdiForm(string caption, string formname)
        {
            bool IsOpened = false;
            foreach (SuperTabItem tabitem in this.tabMain.Tabs)
            {
                if (tabitem.Name == caption)
                {
                    tabMain.SelectedTab = tabitem;
                    IsOpened = true;
                    break;
                }
            }   
            if (!IsOpened)
            {
                object obj = Assembly.GetExecutingAssembly().CreateInstance("MyKTV_Management_Studio." + formname, false);
                Form form = (Form)obj;
                form.TopLevel = false;
                form.Visible = true;
                form.Dock = DockStyle.Fill;
                SuperTabItem item = tabMain.CreateTab(caption);
                item.Text = caption;
                item.Name = caption;
                item.AttachedControl.Controls.Add(form);
                tabMain.SelectedTab = item;
            }
        }

        private void tabMain_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            this.tabMain.Visible = false;
        }

        private void tbiDateTime_Click(object sender, EventArgs e)
        {
            Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL timedate.cpl,,0");
        }
    }
}
