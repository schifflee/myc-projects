namespace WindowsFormsApp3
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.songNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pinYinDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wordCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.singerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.playCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.songBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "全部",
            "热门流行",
            "影视金曲"});
            this.comboBox1.Location = new System.Drawing.Point(86, 56);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.songNameDataGridViewTextBoxColumn,
            this.pinYinDataGridViewTextBoxColumn,
            this.wordCountDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.singerDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn,
            this.playCountDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.songBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(45, 99);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(684, 311);
            this.dataGridView1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(225, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // songNameDataGridViewTextBoxColumn
            // 
            this.songNameDataGridViewTextBoxColumn.DataPropertyName = "SongName";
            this.songNameDataGridViewTextBoxColumn.HeaderText = "SongName";
            this.songNameDataGridViewTextBoxColumn.Name = "songNameDataGridViewTextBoxColumn";
            this.songNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pinYinDataGridViewTextBoxColumn
            // 
            this.pinYinDataGridViewTextBoxColumn.DataPropertyName = "PinYin";
            this.pinYinDataGridViewTextBoxColumn.HeaderText = "PinYin";
            this.pinYinDataGridViewTextBoxColumn.Name = "pinYinDataGridViewTextBoxColumn";
            this.pinYinDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // wordCountDataGridViewTextBoxColumn
            // 
            this.wordCountDataGridViewTextBoxColumn.DataPropertyName = "WordCount";
            this.wordCountDataGridViewTextBoxColumn.HeaderText = "WordCount";
            this.wordCountDataGridViewTextBoxColumn.Name = "wordCountDataGridViewTextBoxColumn";
            this.wordCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // typeDataGridViewTextBoxColumn
            // 
            this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            this.typeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // singerDataGridViewTextBoxColumn
            // 
            this.singerDataGridViewTextBoxColumn.DataPropertyName = "Singer";
            this.singerDataGridViewTextBoxColumn.HeaderText = "Singer";
            this.singerDataGridViewTextBoxColumn.Name = "singerDataGridViewTextBoxColumn";
            this.singerDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url";
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            this.urlDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // playCountDataGridViewTextBoxColumn
            // 
            this.playCountDataGridViewTextBoxColumn.DataPropertyName = "PlayCount";
            this.playCountDataGridViewTextBoxColumn.HeaderText = "PlayCount";
            this.playCountDataGridViewTextBoxColumn.Name = "playCountDataGridViewTextBoxColumn";
            this.playCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // songBindingSource
            // 
            this.songBindingSource.DataSource = typeof(WindowsFormsApp3.Song);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn songNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pinYinDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wordCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn singerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn playCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource songBindingSource;
        private System.Windows.Forms.Button button1;
    }
}

