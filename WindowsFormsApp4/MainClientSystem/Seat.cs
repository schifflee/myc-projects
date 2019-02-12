using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MainClientSystem
{
    public class Seat
    {
        public string SeatNum { get; set; }
        public Color Color { get; set; }

        public Seat()
        {

        }

        public Seat(string seatNum, Color color)
        {
            this.SeatNum = seatNum;
            this.Color = color;
        }

        
    }
}