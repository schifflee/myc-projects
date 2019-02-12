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

namespace 计算机快速设置
{
    public partial class Form2 : Office2007Form
    {
        public Form2()
        {
            EnableGlass = false;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            InitializeUserGroups();
        }

        private void InitializeUserGroups()
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }
    }
}
