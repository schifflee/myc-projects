using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents;
using DevComponents.DotNetBar.Controls;

namespace WindowsFormsApp2 bv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                //MessageBox.Show(this.dataGridViewX1.Rows[this.dataGridViewX1.CurrentRow.Index].Cells[1].Value.ToString());
                //string opt_edit = this.dataGridViewX1.Rows[this.dataGridViewX1.CurrentRow.Index].Cells[0].Value.ToString();
                if (this.dataGridViewX1.Columns[e.ColumnIndex].Name == "edit")
                {
                    if (this.dataGridViewX1.Columns[e.ColumnIndex] is DataGridViewButtonXColumn)
                    {
                        if (this.dataGridViewX1.Rows[this.dataGridViewX1.CurrentRow.Index].Cells["edit"].Value==null)
                        {
                            this.dataGridViewX1.Rows[this.dataGridViewX1.CurrentRow.Index].Cells["edit"].Value = "保存";
                            this.dataGridViewX1.CurrentCell = this.dataGridViewX1.Rows[this.dataGridViewX1.CurrentRow.Index].Cells[0];
                            this.dataGridViewX1.BeginEdit(false);
                        }
                        else
                        {                           
                            this.dataGridViewX1.Rows[this.dataGridViewX1.CurrentRow.Index].Cells["edit"].Value = null;
                            this.dataGridViewX1.CurrentCell = null; 
                            this.button1.Focus();
                        }
                    }
                }
            }
        }
    }
}

