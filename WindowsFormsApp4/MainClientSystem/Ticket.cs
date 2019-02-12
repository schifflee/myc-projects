using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MainClientSystem
{
    public class Ticket
    {
        public double Price { get; set; }
        public ScheduleItem ScheduleItem { get; set; }
        public Seat Seat { get; set; } 
        public Booker Booker { get; set; }

        public Ticket(ScheduleItem scheduleItem, Booker booker, Seat seat)
        {
            this.ScheduleItem = scheduleItem;
            this.Booker = booker;
            this.Seat = seat;
        }

        public Ticket(double price, ScheduleItem scheduleItem, Booker booker, Seat seat)
        {
            this.Price = CalcPrice(price);
            this.ScheduleItem = scheduleItem;
            this.Booker = booker;
            this.Seat = seat;
        }

        public virtual double CalcPrice(double price)
        {
            return price;
        }

        public virtual void Print(out string text)
        {            
            string msg =string.Empty;
            if (!Directory.Exists(Program.TMPPath))
            {
                Directory.CreateDirectory(Program.TMPPath);
            }
            Program.ShortTimeStr = DateTime.Now.ToString("yyyyMMddHHmmss");
            using (FileStream fs=new FileStream(string.Format("{0}\\Ticket[{1}].tmp",Program.TMPPath, Program.ShortTimeStr),FileMode.Create))
            {
                try
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    msg = string.Format(@"|{0}|{1}|{2}|{3}|{4}|{5}|{6}", ScheduleItem.Movie.MovieName, ScheduleItem.Time, Seat.SeatNum, this.Price.ToString("f2"),string.Empty, Booker.Name, Booker.PhoneNum);
                    sw.WriteLine(string.Format("{0}{1}",string.Empty,msg));
                    string tmpstr= string.Format("{0}{1}", string.Empty, msg);
                    text = TicketUtil.CreateTicketString(tmpstr);
                    sw.WriteLine("The End");
                    sw.Close();
                    fs.Close();                    
                }
                catch (IOException)
                {
                    throw new IOException();
                }
            }            
        }

        public virtual void Show(out string text)
        {
            text = "已售出：\n"+
                   "普通票";
        }
    }

    public class TempFile
    {
        public string FileName { get; set; }
        public string FullFileName { get; set; }
        public DateTime FileCreateTime { get; set; }
    }
}
