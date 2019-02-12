using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBWatcher
{
    public class Partition
    {
        public int Index { get; set; }
        public string Letter { get; set; }
        public string Volume { get; set; }
        public string Format { get; set; }
        public long TotalSpace { get; set; }
        public long FreeSpace { get; set; }

        public Partition()
        {

        }

        public Partition(int index, string letter, string volume, string format, long totalSpace, long freeSpace)
        {
            this.Index = index;
            this.Letter = letter;
            this.Volume = volume;
            this.Format = format;
            this.TotalSpace = totalSpace;
            this.FreeSpace = freeSpace;
        }
    }
}
