using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;

namespace MainClientSystem
{
    public class TicketUtil
    {
        public static Ticket CreateTicket(ScheduleItem scheduleItem,Seat seat,double price,int discount,string type,string customerName, string name, string phonenum)
        {
            Ticket ticket = null;
            Booker booker = new Booker(name,phonenum);
            switch (type)
            {
                case "学生票":
                    ticket = new StudentTicket(price, discount, scheduleItem, booker, seat);
                    break;
                case "赠票":
                    ticket = new FreeTicket(price, scheduleItem, booker, seat, customerName);
                    break;
                case "普通票":
                    ticket = new Ticket(price, scheduleItem, booker, seat);
                    break;
            }
            return ticket;
        }      
       
        public static List<TempFile> GetTempTicketFiles(string path)
        {
            List<TempFile> tempfiles = new List<TempFile>();
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Extension.ToUpper()==".TMP")
                {
                    tempfiles.Add(new TempFile
                    {
                        FileName = file.Name,
                        FullFileName = file.FullName,
                        FileCreateTime = file.CreationTime
                    });
                }
            }           
            return tempfiles;
        }

        public static string GetLastestTicketFullFileName(string path)
        {
            var query = from x in GetTempTicketFiles(path) orderby x.FileCreateTime select x;
            TempFile lastestticket = query.LastOrDefault() ;
            return lastestticket.FullFileName;
        }

        public static Ticket GetLastestTicket(string path)
        {
            Ticket ticket = null;
            using (FileStream fs=new FileStream(GetLastestTicketFullFileName(path),FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
            {
                try
                {
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    string line = sr.ReadLine();
                    string[] propertyvalues;
                    while (line.Trim() != "The End")
                    {
                        propertyvalues = line.Split('|');
                        string typestr = propertyvalues[0];
                        string customername = propertyvalues[5];
                        Movie movie = new Movie
                        {
                            MovieName = propertyvalues[1]
                        };
                        ScheduleItem scheduleitem = new ScheduleItem(propertyvalues[2], movie);
                        switch (typestr)
                        {
                            case "free":
                                ticket = new FreeTicket(0, scheduleitem, null, null, null);
                                break;
                            case "student":
                                ticket = new StudentTicket(0, 0, scheduleitem, null, null);
                                break;
                            default:
                                ticket = new Ticket(scheduleitem, null, null);
                                break;
                        }

                        line = sr.ReadLine();                        
                    }
                    sr.Close();
                    fs.Close();
                    return ticket;
                }
                catch (IOException)
                {
                    return null;
                    throw new Exception();
                }
            }
        }

        public static string CreateTicketString(string simpleinfo)
        {
            StringBuilder sb = new StringBuilder();
            string title = Properties.Settings.Default.TicketTitle;
            string[] properties = simpleinfo.Split('|');
            string typestr = properties[0];
            switch (typestr)
            {
                case "free":                                      
                    sb.AppendLine("***********************");
                    sb.AppendLine(string.Format("       {0}(赠票)", title));
                    sb.AppendLine("-----------------------");
                    sb.AppendLine(string.Format("电影：   {0}", properties[1]));
                    sb.AppendLine(string.Format("时间：   {0}", properties[2]));
                    sb.AppendLine(string.Format("座位号： {0}", properties[3]));
                    sb.AppendLine(string.Format("票价：   ￥{0}(免费)", properties[4]));
                    sb.AppendLine(string.Format("赠予人： {0}", properties[5]));
                    sb.AppendLine("***********************");
                    sb.AppendLine("      (b)赠票");
                    break;
                case "student":               
                    sb.AppendLine("***********************");
                    sb.AppendLine(string.Format("       {0}(学生)", title));
                    sb.AppendLine("-----------------------");
                    sb.AppendLine(string.Format("电影：   {0}", properties[1]));
                    sb.AppendLine(string.Format("时间：   {0}", properties[2]));
                    sb.AppendLine(string.Format("座位号： {0}", properties[3]));
                    sb.AppendLine(string.Format("票价：   ￥{0}", properties[4]));
                    sb.AppendLine("***********************");
                    sb.AppendLine("      (c)学生票");
                    break;
                default:
                    sb.AppendLine("***********************");
                    sb.AppendLine(string.Format("       {0}", title));
                    sb.AppendLine("-----------------------");
                    sb.AppendLine(string.Format("电影：   {0}", properties[1]));
                    sb.AppendLine(string.Format("时间：   {0}", properties[2]));
                    sb.AppendLine(string.Format("座位号： {0}", properties[3]));
                    sb.AppendLine(string.Format("票价：   ￥{0}", properties[4]));
                    sb.AppendLine("***********************");
                    sb.AppendLine("      (a)普通票");
                    break;
            }
            return sb.ToString();
        }

        public static int GetFreeTicketCount(string time)
        {
            Cinema cinema = new Cinema();
            cinema.Load(out List<string> textinfos);            
            if (textinfos == null)
            {
                return 0;
            }
            int count = 0;
            foreach (string text in textinfos)
            {
                if (text.Contains(time) && text.Contains("free"))
                {
                    count++;
                }
            }
            return count;
        }

        public static bool DeleteTempTicketFile(string path)
        {
            try
            {
                File.Delete(GetLastestTicketFullFileName(path));
                return true;
            }
            catch (IOException)
            {
                return false;
                throw new IOException();
            }
        }
    }
}
