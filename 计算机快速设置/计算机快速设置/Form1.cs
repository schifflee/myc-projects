using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.WinForms;
using System.Security.Principal;
using Sunisoft.IrisSkin;

namespace 计算机快速设置
{  
    public partial class Form1 : OfficeForm
    {
        public delegate void DelShowSaveFileDialog();

        private string filter = Properties.Resources.ExtFilter;
        private string error1 = Properties.Resources.ErrorString1;
        private string error2 = Properties.Resources.ErrorString2;
        private string error3 = Properties.Resources.ErrorString3;
        private string prompt1 = Properties.Resources.PromptString1;

        public Form1()
        {
            this.EnableGlass = false;               
            InitializeComponent();
        }  

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize();
            //this.skinEngine1.SkinFile = AppDomain.CurrentDomain.BaseDirectory + @"\Skins\vista2_color1.ssk";
        }
       

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = CreateSaveFileDialog(filter, 1, true, true);
            DelShowSaveFileDialog dssfd = () =>
            {
                if (sfd.ShowDialog()==DialogResult.OK)
                {
                    //FileUtil.ExporttoTXT(, sfd.FileName);
                }
            };
        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = CreateSaveFileDialog(filter, 2, true, true);
            DelShowSaveFileDialog dssfd = () =>
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //FileUtil.ExporttoINI(richTextBoxEx5.Text, sfd.FileName);
                }
            };
        }

        private SaveFileDialog CreateSaveFileDialog(string filter, int filterindex, bool checkfileexists, bool checkpathexists)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = filter;
            sfd.FilterIndex = filterindex;
            sfd.CheckFileExists = checkfileexists;
            sfd.CheckPathExists = checkpathexists;
            return sfd;
        }

        private void Initialize()
        { 
            ComputerSettingBiz.GetComputerInfo();
            ComputerSettingBiz.GetNetworkStatus();
            ComputerSettingBiz.GetNetWorkInfo();
            ComputerSettingBiz.SplitPhysicalNetworkAdapters();
            ComputerSettingBiz.GetComputerType();
            ComputerSettingBiz.GetComputerBrand();
            ComputerSettingBiz.GetComputerTypeImage(DeviceImages);
            ComputerSettingBiz.GetSoftwareInfo();
            this.pictureBox1.Image = ComputerSettingBiz.computertypeimage;
            this.labelX2.Text = string.Format("{0} {1}", ComputerSettingBiz.computerbrand, ComputerSettingBiz.computertype);
            this.labelX3.Text = string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4} RAM", ComputerSettingBiz.software.OSName,ComputerSettingBiz.software.OSArch,ComputerSettingBiz.software.OSVersion, ComputerSettingBiz.hardware.CPU, ComputerSettingBiz.hardware.RAM);
            this.labelX4.Text = string.Format("已安装{0}个.NET Framework：\r\n",ComputerSettingBiz.software.DotNetFrameworkVersions.Count);
            foreach (string version in ComputerSettingBiz.software.DotNetFrameworkVersions)
            {
                this.labelX4.Text += string.Format(".NET Framework {0}\r\n",version);
            }
            this.textBoxX1.Text = ComputerSettingBiz.computer.HostName;
            this.textBoxX2.Text = ComputerSettingBiz.computer.User;
            this.textBoxX3.Text = string.Format("CPU：{0}；安装内存：{1}；显卡：{2}", ComputerSettingBiz.hardware.CPU,ComputerSettingBiz.hardware.RAM,ComputerSettingBiz.hardware.GPU);
            this.textBoxX4.Text = ComputerSettingBiz.network.ConnectionState;
            if (!NetworkInfoUtil.IsNetworkAdapterAvailable())
            {
                this.listViewEx2.Visible = false;
                this.tableLayoutPanel1.Visible = false;
            }
            else
            {
                this.listViewEx2.Visible = true;
                this.tableLayoutPanel1.Visible = true;
                InitializeInfo();
                if (listViewEx2.SelectedItems.Count == 0)
                {
                    this.listViewEx2.ContextMenuStrip = this.contextMenuStrip2;
                }
                else
                {
                    this.listViewEx2.ContextMenuStrip = this.contextMenuStrip1;
                }
            }
            InitializeADName();
        }

        private void InitializeInfo()
        {
            //向listViewEx2中添加内容
            this.listViewEx2.SelectedItems.Clear();
            this.listViewEx2.TileSize = new Size(this.listViewEx2.Width, 45);
            ListViewItem lvi;
            foreach (NetworkAdapter adapter in ComputerSettingBiz.physicalnetworkadapters)
            {
                lvi = new ListViewItem();
                lvi.Tag = adapter;
                lvi.Text = adapter.Name;
                lvi.ImageIndex = 0;
                lvi.SubItems.Add(adapter.Description);
                this.listViewEx2.Items.Add(lvi);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonX2_Click(object sender, EventArgs e)
        { 
            
            if (this.buttonX2.Tooltip=="修改计算机名称")
            {
                this.textBoxX1.ReadOnly = false;
                this.textBoxX1.Focus();
                this.textBoxX1.SelectionStart = 0;
                this.textBoxX1.SelectionLength = this.textBoxX1.Text.Length;
                this.textBoxX1.ScrollToCaret();
                this.buttonX2.Image = Properties.Resources.save;
                this.buttonX2.Tooltip = "保存修改";
            }
            else 
            {
                this.buttonX2.Image = Properties.Resources.edit;
                this.buttonX2.Tooltip = "修改计算机名称";
                if (this.textBoxX1.Text.Length == 0)
                {
                    MessageBox.Show(error1, MSG_STRINGS.ERRORTITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (this.textBoxX1.Text == ComputerSettingBiz.computer.HostName)
                {
                    MessageBox.Show(prompt1, MSG_STRINGS.PROMPTTITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }
        }

        private void listViewEx2_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listViewEx2.SelectedItems.Count == 0)
            {
                this.listViewEx2.ContextMenuStrip = this.contextMenuStrip2;
            }
            else
            {
                this.listViewEx2.ContextMenuStrip = this.contextMenuStrip1;
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {

        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void InitializeADName()
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.SelectedIndex==1)
            {
                this.textBoxX8.Enabled = true;
                this.textBoxX9.Enabled = false;
                this.linkLabel1.Enabled = false;
            }
            else if (this.comboBoxEx1.SelectedIndex == 2)
            {
                this.textBoxX8.Enabled = false;
                this.textBoxX9.Enabled = true;
                this.linkLabel1.Enabled = true;
            }
            else
            {
                this.textBoxX8.Enabled = true;
                this.textBoxX9.Enabled = true;
            }
        }
    }
}
