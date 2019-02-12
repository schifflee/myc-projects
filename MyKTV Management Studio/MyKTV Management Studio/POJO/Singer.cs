using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    [Serializable]
    public class Singer
    {
        public int Id { get; set; }
        public string SingerName { get; set; }
        public string PinYin { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string PhotoUrl { get; set; }
        public string Description { get; set; }
    }
}
