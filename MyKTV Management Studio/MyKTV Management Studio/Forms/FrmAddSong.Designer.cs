namespace MyKTV_Management_Studio
{
    partial class FrmAddSong
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.bar2 = new DevComponents.DotNetBar.Bar();
            this.tbiFindSongs = new DevComponents.DotNetBar.ButtonItem();
            this.tbiFindSinger = new DevComponents.DotNetBar.ButtonItem();
            this.tbiAddSong = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem4 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem5 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem6 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem8 = new DevComponents.DotNetBar.ButtonItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.bar2)).BeginInit();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Silver;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154))))));
            // 
            // bar2
            // 
            this.bar2.AccessibleDescription = "bar2 (bar2)";
            this.bar2.AccessibleName = "bar2";
            this.bar2.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.bar2.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar2.DockLine = 1;
            this.bar2.FadeEffect = true;
            this.bar2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.bar2.IsMaximized = false;
            this.bar2.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tbiFindSongs,
            this.tbiFindSinger,
            this.tbiAddSong,
            this.buttonItem3,
            this.buttonItem4,
            this.buttonItem5,
            this.buttonItem6,
            this.buttonItem8});
            this.bar2.Location = new System.Drawing.Point(0, 0);
            this.bar2.Name = "bar2";
            this.bar2.RoundCorners = false;
            this.bar2.Size = new System.Drawing.Size(1069, 33);
            this.bar2.Stretch = true;
            this.bar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar2.TabIndex = 15;
            this.bar2.TabNavigation = true;
            this.bar2.TabStop = false;
            this.bar2.Text = "bar2";
            // 
            // tbiFindSongs
            // 
            this.tbiFindSongs.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.tbiFindSongs.ImageFixedSize = new System.Drawing.Size(24, 24);
            this.tbiFindSongs.Name = "tbiFindSongs";
            this.tbiFindSongs.Text = "查找歌曲";
            // 
            // tbiFindSinger
            // 
            this.tbiFindSinger.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.tbiFindSinger.ImageFixedSize = new System.Drawing.Size(24, 24);
            this.tbiFindSinger.Name = "tbiFindSinger";
            this.tbiFindSinger.Text = "查找歌手";
            // 
            // tbiAddSong
            // 
            this.tbiAddSong.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.tbiAddSong.ImageFixedSize = new System.Drawing.Size(24, 24);
            this.tbiAddSong.Name = "tbiAddSong";
            this.tbiAddSong.Text = "新增歌曲";
            // 
            // buttonItem3
            // 
            this.buttonItem3.Name = "buttonItem3";
            this.buttonItem3.Text = "buttonItem3";
            // 
            // buttonItem4
            // 
            this.buttonItem4.Name = "buttonItem4";
            this.buttonItem4.Text = "buttonItem4";
            // 
            // buttonItem5
            // 
            this.buttonItem5.Name = "buttonItem5";
            this.buttonItem5.Text = "buttonItem5";
            // 
            // buttonItem6
            // 
            this.buttonItem6.Name = "buttonItem6";
            this.buttonItem6.Text = "buttonItem6";
            // 
            // buttonItem8
            // 
            this.buttonItem8.Name = "buttonItem8";
            this.buttonItem8.Text = "buttonItem8";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1069, 767);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // FrmAddSong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 800);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.bar2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmAddSong";
            this.Text = "FrmAddSong";
            this.Load += new System.EventHandler(this.FrmAddSong_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.Bar bar2;
        private DevComponents.DotNetBar.ButtonItem tbiFindSongs;
        private DevComponents.DotNetBar.ButtonItem tbiFindSinger;
        private DevComponents.DotNetBar.ButtonItem tbiAddSong;
        private DevComponents.DotNetBar.ButtonItem buttonItem3;
        private DevComponents.DotNetBar.ButtonItem buttonItem4;
        private DevComponents.DotNetBar.ButtonItem buttonItem5;
        private DevComponents.DotNetBar.ButtonItem buttonItem6;
        private DevComponents.DotNetBar.ButtonItem buttonItem8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}