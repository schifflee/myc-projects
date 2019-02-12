namespace USBWatcher
{
    partial class USBWatcher
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.开始监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.niUSBMonitor = new System.Windows.Forms.NotifyIcon(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem4 = new DevComponents.DotNetBar.ButtonItem();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblPartitionFormat = new DevComponents.DotNetBar.LabelX();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.lblFreeSpace = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.lblTotalSpace = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.lblDiskType = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.lblPartitionIndex = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.lblDiskIndex = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.bgwLoad = new System.ComponentModel.BackgroundWorker();
            this.bgwRefresh = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.expandablePanel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始监控ToolStripMenuItem,
            this.停止监控ToolStripMenuItem,
            this.显示窗口ToolStripMenuItem,
            this.隐藏窗口ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 114);
            // 
            // 开始监控ToolStripMenuItem
            // 
            this.开始监控ToolStripMenuItem.Name = "开始监控ToolStripMenuItem";
            this.开始监控ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.开始监控ToolStripMenuItem.Text = "开始监控";
            // 
            // 停止监控ToolStripMenuItem
            // 
            this.停止监控ToolStripMenuItem.Name = "停止监控ToolStripMenuItem";
            this.停止监控ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.停止监控ToolStripMenuItem.Text = "停止监控";
            // 
            // 显示窗口ToolStripMenuItem
            // 
            this.显示窗口ToolStripMenuItem.Name = "显示窗口ToolStripMenuItem";
            this.显示窗口ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.显示窗口ToolStripMenuItem.Text = "显示窗口";
            // 
            // 隐藏窗口ToolStripMenuItem
            // 
            this.隐藏窗口ToolStripMenuItem.Name = "隐藏窗口ToolStripMenuItem";
            this.隐藏窗口ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.隐藏窗口ToolStripMenuItem.Text = "隐藏窗口";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // niUSBMonitor
            // 
            this.niUSBMonitor.Text = "notifyIcon1";
            this.niUSBMonitor.Visible = true;
            this.niUSBMonitor.DoubleClick += new System.EventHandler(this.niUSBMonitor_DoubleClick);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(0, 6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(305, 115);
            this.treeView1.TabIndex = 2;
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 115);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "正在刷新U盘信息，请稍候...";
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.expandablePanel1.Controls.Add(this.itemPanel1);
            this.expandablePanel1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.expandablePanel1.DisabledBackColor = System.Drawing.Color.Empty;
            this.expandablePanel1.Expanded = false;
            this.expandablePanel1.ExpandedBounds = new System.Drawing.Rectangle(0, 123, 305, 131);
            this.expandablePanel1.ExpandOnTitleClick = true;
            this.expandablePanel1.HideControlsWhenCollapsed = true;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 123);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Size = new System.Drawing.Size(305, 26);
            this.expandablePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.Style.GradientAngle = 90;
            this.expandablePanel1.TabIndex = 4;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "更多操作";
            // 
            // itemPanel1
            // 
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.Class = "ItemPanel";
            this.itemPanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemPanel1.DragDropSupport = true;
            this.itemPanel1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.buttonItem2,
            this.buttonItem3,
            this.buttonItem4});
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.itemPanel1.Location = new System.Drawing.Point(0, 26);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.ReserveLeftSpace = false;
            this.itemPanel1.Size = new System.Drawing.Size(305, 0);
            this.itemPanel1.TabIndex = 4;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "打开U盘";
            // 
            // buttonItem2
            // 
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.Text = "创建U盘快捷方式";
            // 
            // buttonItem3
            // 
            this.buttonItem3.Name = "buttonItem3";
            this.buttonItem3.Text = "安全弹出所有U盘";
            // 
            // buttonItem4
            // 
            this.buttonItem4.Name = "buttonItem4";
            this.buttonItem4.Text = "退出程序";
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2016;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199))))));
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.lblPartitionFormat);
            this.groupPanel1.Controls.Add(this.line1);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.progressBarX1);
            this.groupPanel1.Controls.Add(this.lblFreeSpace);
            this.groupPanel1.Controls.Add(this.labelX9);
            this.groupPanel1.Controls.Add(this.lblTotalSpace);
            this.groupPanel1.Controls.Add(this.labelX7);
            this.groupPanel1.Controls.Add(this.lblDiskType);
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.lblPartitionIndex);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.lblDiskIndex);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.DisabledBackColor = System.Drawing.Color.Empty;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(0, 155);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(305, 156);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 8;
            this.groupPanel1.Text = "信息概览";
            // 
            // lblPartitionFormat
            // 
            // 
            // 
            // 
            this.lblPartitionFormat.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPartitionFormat.Location = new System.Drawing.Point(209, 28);
            this.lblPartitionFormat.Name = "lblPartitionFormat";
            this.lblPartitionFormat.Size = new System.Drawing.Size(55, 23);
            this.lblPartitionFormat.TabIndex = 13;
            this.lblPartitionFormat.Text = "4";
            // 
            // line1
            // 
            this.line1.ForeColor = System.Drawing.Color.Silver;
            this.line1.Location = new System.Drawing.Point(21, 49);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(260, 8);
            this.line1.TabIndex = 12;
            this.line1.Text = "line1";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(144, 30);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 18);
            this.labelX2.TabIndex = 11;
            this.labelX2.Text = "分区格式：";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // progressBarX1
            // 
            // 
            // 
            // 
            this.progressBarX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarX1.Location = new System.Drawing.Point(21, 109);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(260, 21);
            this.progressBarX1.TabIndex = 10;
            this.progressBarX1.Text = "progressBarX1";
            // 
            // lblFreeSpace
            // 
            // 
            // 
            // 
            this.lblFreeSpace.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFreeSpace.Location = new System.Drawing.Point(86, 83);
            this.lblFreeSpace.Name = "lblFreeSpace";
            this.lblFreeSpace.Size = new System.Drawing.Size(55, 23);
            this.lblFreeSpace.TabIndex = 9;
            this.lblFreeSpace.Text = "6";
            // 
            // labelX9
            // 
            this.labelX9.AutoSize = true;
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Location = new System.Drawing.Point(21, 85);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(68, 18);
            this.labelX9.TabIndex = 8;
            this.labelX9.Text = "可用空间：";
            this.labelX9.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lblTotalSpace
            // 
            // 
            // 
            // 
            this.lblTotalSpace.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTotalSpace.Location = new System.Drawing.Point(86, 59);
            this.lblTotalSpace.Name = "lblTotalSpace";
            this.lblTotalSpace.Size = new System.Drawing.Size(55, 23);
            this.lblTotalSpace.TabIndex = 7;
            this.lblTotalSpace.Text = "5";
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(21, 61);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(68, 18);
            this.labelX7.TabIndex = 6;
            this.labelX7.Text = "空间大小：";
            this.labelX7.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lblDiskType
            // 
            // 
            // 
            // 
            this.lblDiskType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDiskType.Location = new System.Drawing.Point(209, 3);
            this.lblDiskType.Name = "lblDiskType";
            this.lblDiskType.Size = new System.Drawing.Size(55, 23);
            this.lblDiskType.TabIndex = 5;
            this.lblDiskType.Text = "3";
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(144, 5);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(68, 18);
            this.labelX6.TabIndex = 4;
            this.labelX6.Text = "磁盘类型：";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lblPartitionIndex
            // 
            // 
            // 
            // 
            this.lblPartitionIndex.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPartitionIndex.Location = new System.Drawing.Point(86, 28);
            this.lblPartitionIndex.Name = "lblPartitionIndex";
            this.lblPartitionIndex.Size = new System.Drawing.Size(55, 23);
            this.lblPartitionIndex.TabIndex = 3;
            this.lblPartitionIndex.Text = "2";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(45, 30);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(44, 18);
            this.labelX4.TabIndex = 2;
            this.labelX4.Text = "分区：";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lblDiskIndex
            // 
            // 
            // 
            // 
            this.lblDiskIndex.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblDiskIndex.Location = new System.Drawing.Point(86, 3);
            this.lblDiskIndex.Name = "lblDiskIndex";
            this.lblDiskIndex.Size = new System.Drawing.Size(55, 23);
            this.lblDiskIndex.TabIndex = 1;
            this.lblDiskIndex.Text = "1";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(45, 5);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(44, 18);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "磁盘：";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // bgwLoad
            // 
            this.bgwLoad.WorkerReportsProgress = true;
            this.bgwLoad.WorkerSupportsCancellation = true;
            this.bgwLoad.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwLoad_DoWork);
            this.bgwLoad.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwLoad_RunWorkerCompleted);
            // 
            // bgwRefresh
            // 
            this.bgwRefresh.WorkerReportsProgress = true;
            this.bgwRefresh.WorkerSupportsCancellation = true;
            this.bgwRefresh.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRefresh_DoWork);
            this.bgwRefresh.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRefresh_RunWorkerCompleted);
            // 
            // USBWatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BottomLeftCornerSize = 2;
            this.BottomRightCornerSize = 2;
            this.ClientSize = new System.Drawing.Size(305, 311);
            this.Controls.Add(this.expandablePanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.treeView1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "USBWatcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.TitleText = "USBWatcher";
            this.TopLeftCornerSize = 2;
            this.TopRightCornerSize = 2;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.USBWatcher_FormClosing);
            this.Load += new System.EventHandler(this.USBWatcher_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.expandablePanel1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 开始监控ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止监控ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon niUSBMonitor;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.ButtonItem buttonItem3;
        private DevComponents.DotNetBar.ButtonItem buttonItem4;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.ComponentModel.BackgroundWorker bgwLoad;
        private System.ComponentModel.BackgroundWorker bgwRefresh;
        private DevComponents.DotNetBar.LabelX lblDiskType;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX lblPartitionIndex;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX lblDiskIndex;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX lblFreeSpace;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.LabelX lblTotalSpace;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private DevComponents.DotNetBar.LabelX lblPartitionFormat;
        private DevComponents.DotNetBar.Controls.Line line1;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}

