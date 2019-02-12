using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainClientSystem
{
    public class ScheduleItem
    {
        public string Time { get; set; }
        public Movie Movie { get; set; }

        public ScheduleItem()
        {

        }

        public ScheduleItem(string time, Movie movie)
        {
            this.Time = time;
            this.Movie = movie;
        }
    }
}