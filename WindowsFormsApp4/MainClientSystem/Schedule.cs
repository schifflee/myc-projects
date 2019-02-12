using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MainClientSystem
{
    public class Schedule
    {
        public Dictionary<string, ScheduleItem> Items { get; set; }

        public Schedule()
        {
            Items = new Dictionary<string, ScheduleItem>();
        }

        public void LoadItems()
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(Program.XMLFilePath);
                XmlElement element = document.DocumentElement;
                ScheduleItem scheduleitem = null;
                foreach (XmlNode node in element.ChildNodes)
                {
                    foreach (XmlNode subNode in node["Schedule"].ChildNodes)
                    {
                        Movie movie = new Movie
                        {
                            Actor = node["Actor"].InnerText,
                            Director = node["Director"].InnerText,
                            MovieName = node["Name"].InnerText,
                            Poster = node["Poster"].InnerText,                           
                            MovieType = (MovieType)Enum.Parse(typeof(MovieType), node["Type"].InnerText),
                            Summary=node["Summary"].InnerText,
                            Price = Convert.ToDouble(node["Price"].InnerText),
                        };
                        string time = subNode.InnerText;
                        scheduleitem = new ScheduleItem(time, movie);                       
                        Items.Add(time, scheduleitem);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}