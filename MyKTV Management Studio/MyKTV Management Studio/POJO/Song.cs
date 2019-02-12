using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public class Song
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public string PinYin { get; set; }
        public int WordCount { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string SingerId { get; set; }
        public string Singer { get; set; }
        public string Url{ get; set; }
        public int PlayCount { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
