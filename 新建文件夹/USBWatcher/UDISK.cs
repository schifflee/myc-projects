using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBWatcher
{
    public class UDISK
    {
        public int DiskIndex { get; set; }
        public string Caption { get; set; }
        public string DiskType { get; set; }
        public List<string> Letters { get; set; }
        public long TotalSpace { get; set; }

        public List<Partition> Partitions { get; set; }

        public UDISK()
        {

        }

        public UDISK(int diskIndex, string caption, string diskType, List<string> letters, long totalSpace)
        {
            this.DiskIndex = diskIndex;
            this.Caption = caption;
            this.DiskType = diskType;
            this.Letters = letters;
            this.TotalSpace = totalSpace;
        }

        public UDISK(int diskIndex,List<Partition> partitions)
        {
            this.DiskIndex = diskIndex;
            this.Partitions = partitions;
        }
    }
}
