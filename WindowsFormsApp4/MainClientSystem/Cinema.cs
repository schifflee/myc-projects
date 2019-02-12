using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainClientSystem
{
    public class Cinema
    {
        public Dictionary<string, Seat> Seats { get; set; }
        public Schedule Schedule { get; set; }
        public List<Ticket> SoldTicket { get; set; }

        public void Save()
        {
            using (FileStream fs = new FileStream(Program.TicketSavedFilePath, FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                string message = string.Empty;
                try
                {
                    foreach (Ticket item in SoldTicket)
                    {
                        message = string.Format(@"|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", item.ScheduleItem.Movie.MovieName, item.ScheduleItem.Movie.Poster,
                            item.ScheduleItem.Movie.Director, item.ScheduleItem.Movie.Actor, item.ScheduleItem.Movie.MovieType.ToString(), item.ScheduleItem.Movie.Summary, item.ScheduleItem.Movie.Price.ToString("f2"),
                            item.ScheduleItem.Time, item.Seat.SeatNum, ColorTranslator.ToHtml(item.Seat.Color).ToString(), item.Booker.Name, item.Booker.PhoneNum);
                        if (item is FreeTicket)
                        {
                            string customername = ((FreeTicket)item).CustomerName;
                            sw.WriteLine(string.Format("free{0}{1}", message, customername));
                        }
                        else if (item is StudentTicket)
                        {
                            sw.WriteLine(string.Format("student{0}{1}", message, string.Empty));
                        }
                        else
                        {
                            sw.WriteLine(string.Format("{0}{1}{2}", string.Empty, message, string.Empty));
                        }
                    }
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

        public void Load(out List<string> texts)
        {
            texts = new List<string>();
            if (!File.Exists(Program.TicketSavedFilePath))
            {
                texts = null;
                return;
            }
            using (FileStream fs = new FileStream(Program.TicketSavedFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                try
                {
                    StreamReader sr = new StreamReader(Program.TicketSavedFilePath, Encoding.GetEncoding("GB2312"));
                    string line = sr.ReadLine();
                    string[] propertyvalues;
                    Ticket ticket = null;
                    while (line.Trim() != "The End")
                    {
                        propertyvalues = line.Split('|');
                        string typestr = propertyvalues[0];
                        Movie movie = new Movie(propertyvalues[1], propertyvalues[2], propertyvalues[3], propertyvalues[4],
                            (MovieType)Enum.Parse(typeof(MovieType), propertyvalues[5]),propertyvalues[6], Convert.ToDouble(propertyvalues[7]));
                        ScheduleItem scheduleitem = new ScheduleItem
                        {
                            Time = propertyvalues[8],
                            Movie = movie
                        };
                        Booker booker = new Booker(propertyvalues[11], propertyvalues[12]);
                        Seat seat = new Seat
                        {
                            SeatNum = propertyvalues[9],
                            Color = ColorTranslator.FromHtml(propertyvalues[10])
                        };
                        string customername = propertyvalues[13];
                        double price = Convert.ToDouble(propertyvalues[7]);
                        int discount = 0;
                        string type = string.Empty;
                        switch (typestr)
                        {
                            case "student":
                                discount = Program.Discount;
                                ticket = new StudentTicket(price, discount, scheduleitem, booker, seat);
                                break;
                            case "free":
                                ticket = new FreeTicket(price, scheduleitem, booker, seat, customername);
                                break;
                            default:
                                ticket = new Ticket(price, scheduleitem, booker, seat);
                                break;
                        }
                        texts.Add(line);
                        this.SoldTicket.Add(ticket);
                        line = sr.ReadLine();
                    }
                    sr.Close();
                    fs.Close();
                }
                catch (IOException)
                {
                    throw new IOException();
                }
            }
        }
    }
}
