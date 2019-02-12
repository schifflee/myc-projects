using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainClientSystem;

namespace MainClientSystem
{
    public partial class FrmMain : Form
    {
        int ControlStatus = 0;
        Cinema cinema = new Cinema();
        List<Movie> movielist = new List<Movie>();
        Dictionary<string, ScheduleItem> scheduleitems;
        string tickettype = string.Empty;
        string customername = string.Empty;
        string name = string.Empty;
        string phonenum = string.Empty;
        Ticket soldticket;
        PrintDocument document;
        Dictionary<string, Label> labels = new Dictionary<string, Label>();
        List<string> ticketinfos;
        ClientModule client = new ClientModule();
        public FrmMain()
        {
            InitializeComponent();
            Program.TicketTitle = Properties.Settings.Default.TicketTitle;
            Program.Discount = Properties.Settings.Default.Discount;
            Program.XPosition = Printer.Default.XPosition;
            Program.YPosition = Printer.Default.YPosition;
            Program.Landscape = Printer.Default.Landscape;
            Printer.Default.DefaultPrinterName = Program.DefaultPrinterName;
            Printer.Default.PaperSize = Program.PaperSize;
            Printer.Default.FontToPrint = Program.FontToPrint;
        }       

        private void label_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Program.TicketInfo = string.Empty;
            Seat seat = null;
            ScheduleItem scheduleitem = null;
            if (label.BackColor == Color.Yellow)
            {
                DialogResult dr = MessageBox.Show("是否订票?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    label.BackColor = Color.DarkOrange;
                    seat = new Seat(label.Text, label.BackColor);
                    scheduleitem = new ScheduleItem(this.tvMovieList.SelectedNode.Text, scheduleitems[this.tvMovieList.SelectedNode.Text].Movie);
                    soldticket = TicketUtil.CreateTicket(scheduleitem, seat, cinema.Schedule.Items[this.tvMovieList.SelectedNode.Text].Movie.Price, Program.Discount, tickettype, customername, name, phonenum);
                    soldticket.Print(out Program.TicketInfo);
                    MessageBox.Show("订票成功，右侧信息栏显示的是您的订票信息，请核对！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cinema.SoldTicket.Add(soldticket);
                    this.groupBox3.Enabled = true;
                    this.richTextBox1.Text = Program.TicketInfo;
                    this.btnPrintTicket.Enabled = true;
                    this.btnModifyTicket.Enabled = true;
                }
                else
                {
                    return;
                }
                ControlStatus = 4;
            }
            else if (label.BackColor == Color.DarkOrange)
            {
                string msg = string.Empty;
                string ticketinfo = GetTicketInfoBySeatNum(label.Text);
                if (ticketinfo.Contains("free"))
                {
                    seat = new Seat(label.Text, label.BackColor);
                    scheduleitem = new ScheduleItem(this.tvMovieList.SelectedNode.Text, scheduleitems[this.tvMovieList.SelectedNode.Text].Movie);
                    soldticket = new FreeTicket(0, scheduleitem, null, seat, ticketinfo.Split('|')[10]);
                    ((FreeTicket)soldticket).Show(out msg);
                    DialogResult dr = MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dr == DialogResult.OK)
                    {
                        return;
                    }
                }
                else if (ticketinfo.Contains("student"))
                {
                    seat = new Seat(label.Text, label.BackColor);
                    scheduleitem = new ScheduleItem(this.tvMovieList.SelectedNode.Text, scheduleitems[this.tvMovieList.SelectedNode.Text].Movie);
                    soldticket = new StudentTicket(0, Program.Discount, scheduleitem, null, seat);
                    ((StudentTicket)soldticket).Show(out msg);
                    DialogResult dr = MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dr == DialogResult.OK)
                    {
                        return;
                    }
                }
                else
                {
                    seat = new Seat(label.Text, label.BackColor);
                    scheduleitem = new ScheduleItem(this.tvMovieList.SelectedNode.Text, scheduleitems[this.tvMovieList.SelectedNode.Text].Movie);
                    soldticket = new Ticket(0, scheduleitem, null, seat);
                    soldticket.Show(out msg);
                    DialogResult dr = MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dr == DialogResult.OK)
                    {
                        return;
                    }
                }
                ControlStatus = 5;
            }
            this.tvMovieList.CollapseAll();
            this.groupBox1.Enabled = false;

        }

        private void tsmOrder_Click(object sender, EventArgs e)
        {
            ControlStatus = 1;
            cinema.Load(out ticketinfos);
            UpdateSeat();
            this.groupBox1.Enabled = true;
            this.tvMovieList.Focus();
        }

        private void LoadMovieItems()
        {
            tvMovieList.BeginUpdate();
            tvMovieList.Nodes.Clear();
            TreeNode movienode = null;
            TreeNode timenode = null;
            cinema.Schedule = new Schedule();
            cinema.Schedule.LoadItems();
            string movieName = string.Empty;
            scheduleitems = cinema.Schedule.Items;
            movielist = scheduleitems.Values.Select(x => x.Movie).ToList();
            List<string> movienamelist = movielist.Select(x => x.MovieName).ToList();
            List<string> timelist = scheduleitems.Keys.ToList();
            foreach (string moviename in movienamelist.Distinct().ToList())
            {
                movienode = new TreeNode
                {
                    Tag = moviename,
                    Text = moviename
                };
                foreach (string time in timelist)
                {
                    timenode = new TreeNode
                    {
                        Tag = time,
                        Text = time
                    };
                    if (scheduleitems.ContainsKey(time))
                    {
                        if (scheduleitems[time].Movie.MovieName == (string)movienode.Tag)
                        {
                            movienode.Nodes.Add(timenode);
                        }
                    }
                }
                this.tvMovieList.Nodes.Add(movienode);
            }
            tvMovieList.EndUpdate();
        }

        private void tvMovieList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ControlStatus = 2;
            this.tsmOrder.Enabled = false;
            this.groupBox2.Enabled = true;
            this.tabControl2.Enabled = false;
            if (e.Node == null) return;
            if (e.Node.Level == 0)
            {
                InitSeatColor();
                this.lblMovieName.Text = movielist.Find(x => x.MovieName == e.Node.Text).MovieName;
                this.lblDirector.Text = movielist.Find(x => x.MovieName == e.Node.Text).Director;
                this.lblActor.Text = movielist.Find(x => x.MovieName == e.Node.Text).Actor;
                this.lblMovieType.Text = movielist.Find(x => x.MovieName == e.Node.Text).MovieType.GetDescriptionName();
                this.lblSummary.Text = movielist.Find(x => x.MovieName == e.Node.Text).Summary;
                this.pbPoster.Image = Image.FromFile(string.Format("{0}\\{1}", Program.PosterFilesPath, movielist.Find(x => x.MovieName == e.Node.Text).Poster));
                this.lblTime.Text = "-";
                this.lblPrice.Text = "-";
                this.lblFavPrice.Text = "-";
            }
            if (e.Node.Level == 1)
            {
                this.panel1.Enabled = true;
                this.lblMovieName.Text = movielist.Find(x => x.MovieName == e.Node.Parent.Text).MovieName;
                this.lblDirector.Text = movielist.Find(x => x.MovieName == e.Node.Parent.Text).Director;
                this.lblActor.Text = movielist.Find(x => x.MovieName == e.Node.Parent.Text).Actor;
                this.lblMovieType.Text = movielist.Find(x => x.MovieName == e.Node.Parent.Text).MovieType.GetDescriptionName();
                this.lblSummary.Text = movielist.Find(x => x.MovieName == e.Node.Text).Summary;
                this.pbPoster.Image = Image.FromFile(string.Format("{0}\\{1}", Program.PosterFilesPath, movielist.Find(x => x.MovieName == e.Node.Parent.Text).Poster));
                this.lblTime.Text = scheduleitems[e.Node.Text].Time;
                this.lblPrice.Text = scheduleitems[e.Node.Text].Movie.Price.ToString("C");
                this.lblFavPrice.Text = "-";
                InitSeatColor();
                foreach (Ticket ticket in cinema.SoldTicket)
                {
                    foreach (Seat seat in cinema.Seats.Values)
                    {
                        if (ticket.ScheduleItem.Time == e.Node.Text && ticket.Seat.SeatNum == seat.SeatNum)
                        {
                            InitSeatColor();
                            seat.Color = Color.DarkOrange;
                        }
                    }
                }
                UpdateSeat();
            }
        }

        private void InitSeats()
        {
            cinema.Seats = new Dictionary<string, Seat>();
            Program.ShowroomRowNum = 5;
            Program.ShowroomColumnNum = 7;
            for (int i = 0; i < Program.ShowroomColumnNum; i++)
            {
                for (int j = 0; j < Program.ShowroomRowNum; j++)
                {
                    Label label = new Label
                    {
                        BackColor = Color.Yellow,
                        Font = new Font("宋体", 14.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)134)),
                        AutoSize = false,
                        Size = new Size(50, 25),
                        Text = string.Format("{0}-{1}", (j + 1).ToString(), (i + 1).ToString()),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(25 + (i * 88), 25 + (j * 50)),
                    };
                    label.Click += new EventHandler(label_Click);
                    tabPage2.Controls.Add(label);
                    Seat seat = new Seat(string.Format("{0}-{1}", (j + 1).ToString(), (i + 1).ToString()), Color.Yellow);
                    labels.Add(seat.SeatNum, label);
                    cinema.Seats.Add(seat.SeatNum, seat);
                }
            }
        }

        private List<RadioButton> GetRadioButton()
        {

            List<RadioButton> radiobuttons = new List<RadioButton>();
            foreach (Control c in this.panel1.Controls)
            {
                if (c is RadioButton)
                {
                    radiobuttons.Add((RadioButton)c);
                }
            }
            return radiobuttons;
        }

        private void InitSeatColor()
        {
            foreach (Seat seat in cinema.Seats.Values)
            {
                seat.Color = Color.Yellow;
            }
        }

        private void UpdateSeat()
        {
            foreach (string key in cinema.Seats.Keys)
            {
                labels[key].BackColor = cinema.Seats[key].Color;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private string GetTicketInfoBySeatNum(string seatNum)
        {
            string ticketinfo = string.Empty;
            foreach (string item in ticketinfos)
            {
                if (item.Contains(seatNum))
                {
                    ticketinfo = item;
                }
            }
            return ticketinfo;
        }

        private void InitControlStatus()
        {
            this.groupBox1.Enabled = false;
            this.groupBox2.Enabled = false;
            this.groupBox3.Enabled = false;
            this.panel1.Enabled = false;
            this.tabControl2.Enabled = false;
            this.btnPrintTicket.Enabled = false;
            this.btnModifyTicket.Enabled = false;
            this.radioButton1.Checked = true;
            this.lblRecieverFomatPrompt.Visible = false;
            tickettype = "普通票";
        }

        private void btnModifyTicket_Click(object sender, EventArgs e)
        {
            Ticket ticket = TicketUtil.GetLastestTicket(Program.TMPPath);
            this.groupBox1.Enabled = true;
            this.tvMovieList.Focus();
            for (int i = 0; i < this.tvMovieList.Nodes.Count; i++)
            {
                for (int j = 0; j < this.tvMovieList.Nodes[i].Nodes.Count; j++)
                {
                    if ((string)this.tvMovieList.Nodes[i].Nodes[j].Tag == ticket.ScheduleItem.Time)
                    {
                        this.tvMovieList.SelectedNode = this.tvMovieList.Nodes[i].Nodes[j];
                        this.tvMovieList.Nodes[i].Expand();
                    }
                }
            }
            foreach (Control c in this.tabPage2.Controls)
            {
                if (c is Label)
                {
                    ((Label)c).BackColor = Color.Yellow;
                }
            }
            this.richTextBox1.Text = string.Empty;
            TicketUtil.DeleteTempTicketFile(Program.TMPPath);
        }

        private void tsmPrinterSettings_Click(object sender, EventArgs e)
        {
            PageSetupDialog pagesetupdailog = new PageSetupDialog();
            DialogResult dr = pagesetupdailog.ShowDialog();
            pagesetupdailog.ShowHelp = true;
            pagesetupdailog.ShowNetwork = true;
            if (dr==DialogResult.OK)
            {
                Printer.Default.PaperSize = pagesetupdailog.PageSettings.PaperSize;
                Printer.Default.XPosition = pagesetupdailog.PageSettings.HardMarginX;
                Printer.Default.YPosition = pagesetupdailog.PageSettings.HardMarginY;
                Printer.Default.Landscape = pagesetupdailog.PageSettings.Landscape;
            }
            
        }
        private void btnPrintTicket_Click(object sender, EventArgs e)
        {
            document = new PrintDocument();
            document.DefaultPageSettings.Landscape = Program.Landscape;
            document.DefaultPageSettings.PaperSize = Program.PaperSize;            
            document.PrintPage += new PrintPageEventHandler(document_PrintPage);
            document.EndPrint += new PrintEventHandler(document_EndPrint);
            document.Print();
        }

        private void document_EndPrint(object sender, PrintEventArgs e)
        {
            MessageBox.Show("打印完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TicketUtil.DeleteTempTicketFile(Program.TMPPath);
        }

        private void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            float xpos = Program.XPosition;
            float ypos = Program.YPosition;
            e.Graphics.DrawString(this.richTextBox1.Text, Program.FontToPrint, Brushes.Black, xpos, ypos);            
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            cinema.SoldTicket = new List<Ticket>();
            InitControlStatus();
            LoadMovieItems();
            InitSeats();
            foreach (RadioButton rb in GetRadioButton())
            {
                rb.CheckedChanged += new EventHandler(radiobutton_CheckedChanged);
            }
            this.cbStudentDiscount.Items.Add(Program.Discount);
            this.cbStudentDiscount.SelectedIndex = 0;
            ControlStatus = 1;
            client.ConnectToServer("192.168.1.124",6000);                   
            MessageBox.Show(client.MSG_Recieve);            
        }

        private void radiobutton_CheckedChanged(object sender, EventArgs e)
        {
            foreach (RadioButton rb in GetRadioButton())
            {
                if (rb.Checked)
                {
                    switch (rb.Name)
                    {
                        case "radioButton1":
                            tickettype = "普通票";
                            this.cbStudentDiscount.Enabled = false;
                            this.txtCustomerName.Enabled = false;
                            this.lblRecieverFomatPrompt.Visible = false;
                            break;
                        case "radioButton2":
                            tickettype = "赠票";
                            string customername = string.Empty;
                            this.cbStudentDiscount.Enabled = false;
                            this.txtCustomerName.Enabled = true;
                            if (TicketUtil.GetFreeTicketCount(this.tvMovieList.SelectedNode.Text) > 1)
                            {
                                MessageBox.Show("赠票已售完，请选择其他票型！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                radioButton1.Checked = true;
                            }
                            this.lblRecieverFomatPrompt.Visible = true;
                            break;
                        case "radioButton3":
                            tickettype = "学生票";
                            this.cbStudentDiscount.Enabled = true;
                            this.txtCustomerName.Enabled = false;
                            this.lblRecieverFomatPrompt.Visible = false;
                            break;
                        default:
                            tickettype = string.Empty;
                            break;
                    }
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ControlStatus = 3;           
            Booker booker = null;
            if (radioButton1.Checked)
            {
                this.txtCustomerName.Text = string.Empty;
                if (this.tvMovieList.SelectedNode.Level == 0)
                {
                    MessageBox.Show("请选择场次！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (this.tvMovieList.SelectedNode.Level == 1)
                {
                    if (string.IsNullOrEmpty(this.txtName.Text.Trim()) || string.IsNullOrEmpty(this.txtName.Text.Trim()))
                    {
                        MessageBox.Show("必须填写姓名和电话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        name = this.txtName.Text.Trim();
                        phonenum = this.txtName.Text.Trim();
                        booker= booker = new Booker(name, phonenum);
                        if (booker.Validate(name, phonenum))
                        {                          
                            if (!booker.IsPhoneNumExists(phonenum))
                            {                                
                                soldticket = new Ticket(scheduleitems[this.tvMovieList.SelectedNode.Text].Movie.Price, scheduleitems[this.tvMovieList.SelectedNode.Text], booker, null);
                                this.lblFavPrice.Text = soldticket.Price.ToString("C");
                                this.txtName.Text = string.Empty;
                                this.txtPhoneNum.Text = string.Empty;
                                this.panel1.Enabled = false;
                                this.radioButton1.Focus();
                            }
                            else
                            {
                                MessageBox.Show("电话号码已存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.txtPhoneNum.Text = string.Empty;
                                this.txtPhoneNum.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("姓名或电话号码格式错误，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.txtName.Text = string.Empty;
                            this.txtPhoneNum.Text = string.Empty;
                            this.txtName.Focus();
                        }
                    }
                }
            }
            if (radioButton2.Checked)
            {
                if (this.tvMovieList.SelectedNode.Level == 0)
                {
                    MessageBox.Show("请选择场次并填写赠予人姓名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (this.tvMovieList.SelectedNode.Level == 1)
                {
                    if (string.IsNullOrEmpty(this.txtCustomerName.Text.Trim()))
                    {
                        MessageBox.Show("请填写赠予人姓名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtCustomerName.Focus();
                    }
                    else
                    {
                        customername = this.txtCustomerName.Text.Trim();
                        if (!StringValidator.IsChineseWord(customername))
                        {
                            MessageBox.Show("赠予人姓名格式错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.txtCustomerName.Text = string.Empty;
                            this.txtCustomerName.Focus();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(this.txtName.Text.Trim()) || string.IsNullOrEmpty(this.txtName.Text.Trim()))
                            {
                                MessageBox.Show("必须填写姓名和电话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                name = this.txtName.Text.Trim();
                                phonenum = this.txtName.Text.Trim();
                                booker = new Booker(name, phonenum);
                                if (booker.Validate(name, phonenum))
                                {
                                    if (!booker.IsPhoneNumExists(phonenum))
                                    {                                        
                                        customername = this.txtCustomerName.Text.Trim();
                                        soldticket = new FreeTicket(scheduleitems[this.tvMovieList.SelectedNode.Text].Movie.Price, scheduleitems[this.tvMovieList.SelectedNode.Text], booker, null, customername);
                                        this.lblFavPrice.Text = soldticket.Price.ToString("C");
                                        this.txtName.Text = string.Empty;
                                        this.txtPhoneNum.Text = string.Empty;
                                        this.panel1.Enabled = false;
                                        this.radioButton1.Focus();
                                    }
                                    else
                                    {
                                        MessageBox.Show("电话号码已存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        this.txtPhoneNum.Text = string.Empty;
                                        this.txtPhoneNum.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("姓名或电话号码格式错误，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    this.txtName.Text = string.Empty;
                                    this.txtPhoneNum.Text = string.Empty;
                                    this.txtName.Focus();
                                }
                            }
                        }                       
                    }
                }
            }
            if (radioButton3.Checked)
            {
                this.txtCustomerName.Text = string.Empty;
                if (this.tvMovieList.SelectedNode.Level == 0)
                {
                    MessageBox.Show("请选择场次！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (this.tvMovieList.SelectedNode.Level == 1)
                {
                    name = this.txtName.Text.Trim();
                    phonenum = this.txtName.Text.Trim();
                    booker = new Booker(name, phonenum);
                    if (booker.Validate(name, phonenum))
                    {
                        if (!booker.IsPhoneNumExists(phonenum))
                        {                           
                            soldticket = new StudentTicket(scheduleitems[this.tvMovieList.SelectedNode.Text].Movie.Price, Program.Discount, scheduleitems[this.tvMovieList.SelectedNode.Text], booker, null);
                            this.lblFavPrice.Text = soldticket.Price.ToString("C");
                            this.txtName.Text = string.Empty;
                            this.txtPhoneNum.Text = string.Empty;
                            this.panel1.Enabled = false;
                            this.radioButton1.Focus();
                        }
                        else
                        {
                            MessageBox.Show("电话号码已存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.txtPhoneNum.Text = string.Empty;
                            this.txtPhoneNum.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("姓名或电话号码格式错误，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.txtName.Text = string.Empty;
                        this.txtPhoneNum.Text = string.Empty;
                        this.txtName.Focus();
                    }
                    
                }
            }
            this.tabControl2.Enabled = true;
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DiscountList == null)
            {
                Properties.Settings.Default.DiscountList = new System.Collections.Specialized.StringCollection();
                foreach (var item in this.cbStudentDiscount.Items)
                {
                    Properties.Settings.Default.DiscountList.Add(item.ToString());
                }
            }
            Properties.Settings.Default.Save();
            if (ControlStatus == 1 || ControlStatus == 2 || ControlStatus == 3 || ControlStatus == 5)
            {
                Environment.Exit(0);
            }
            else if (ControlStatus == 4)
            {
                DialogResult dr = MessageBox.Show("是否要保存订票信息？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    cinema.Save();
                }
                else
                {
                    return;
                }
            }
        }
    }
}
