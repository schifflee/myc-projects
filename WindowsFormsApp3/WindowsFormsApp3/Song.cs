using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApp3
{
    public class Song
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public string PinYin { get; set; }
        public int WordCount { get; set; }
        public string Type { get; set; }
        public string Singer { get; set; }
        public string Url{ get; set; }
        public int PlayCount { get; set; }
    }
}
