using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainClientSystem
{
    public class FreeTicket : Ticket
    {
        public string CustomerName { get; set; }

        public FreeTicket(double price, ScheduleItem scheduleItem, Booker booker, Seat seat, string customerName) : base(scheduleItem, booker, seat)
        {
            this.CustomerName = customerName;
            this.Price = CalcPrice(price);
        }

        public override double CalcPrice(double price)
        {
            return 0;
        }

        public override void Print(out string text)
        {
            string msg = string.Empty;
            if (!Directory.Exists(Program.TMPPath))
            {
                Directory.CreateDirectory(Program.TMPPath);
            }
            Program.ShortTimeStr = DateTime.Now.ToString("yyyyMMddHHmmss");
            using (FileStream fs = new FileStream(string.Format("{0}\\FreeTicket[{1}].tmp", Program.TMPPath, Program.ShortTimeStr), FileMode.OpenOrCreate))
            {
                try
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    msg = string.Format(@"|{0}|{1}|{2}|{3}|{4}|{5}|{6}|", ScheduleItem.Movie.MovieName, ScheduleItem.Time, Seat.SeatNum, this.Price.ToString("f2"), this.CustomerName, Booker.Name, Booker.PhoneNum);
                    sw.WriteLine(string.Format("free{0}", msg));
                    string tmpstr = string.Format("free{0}", msg);
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

        public override void Show(out string text)
        {
            text = "已售出：\n" +
                   string.Format("赠票，赠予人：{0}", this.CustomerName);
        }
    }
}
