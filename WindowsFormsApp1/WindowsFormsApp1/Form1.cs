using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //RadioButton rd =null;
            //int index = 0;
            int rdnum = 0;
            int cbnum = 0;
            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                if (groupBox1.Controls[i] is RadioButton)
                {
                    rdnum++;
                }
                if (groupBox1.Controls[i] is CheckBox)
                {
                    cbnum++;
                }
            }
            RadioButton[] rds = new RadioButton[rdnum];
            CheckBox[] cbs = new CheckBox[cbnum];
            string cname = null;
            for (int i = 0; i < rds.Length; i++)
            {
                for (int j = 0; j < groupBox1.Controls.Count; j++)
                {
                    if (groupBox1.Controls[j] is RadioButton)
                    {
                        rds[i] = (RadioButton)groupBox1.Controls[j];
                    }
                   
                }                
                rds[i].CheckedChanged += new EventHandler(rd_CheckedChanged);                
            }
            for (int i = 0; i < cbs.Length; i++)
            {
                for (int j = 0; j < groupBox1.Controls.Count; j++)
                {
                    if (groupBox1.Controls[j] is CheckBox)
                    {
                        cbs[i] = (CheckBox)groupBox1.Controls[j];
                    }

                }
                cbs[i].CheckedChanged += new EventHandler(cb_CheckedChanged);
            }
            this.textBox1.Text = cname;
            this.treeView1.Nodes.Add("a");
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            if (cb1.Checked)
            {
                this.textBox1.Text = cb1.Name;
            }
        }

        private void rd_CheckedChanged(object sender, EventArgs e)
        {
            if (rd1.Checked)
            {
                this.textBox1.Text = rd1.Name;
            }
            else if (rd2.Checked)
            {
                this.textBox1.Text = rd2.Name;
            }
        }
    }
}
