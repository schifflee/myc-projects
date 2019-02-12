using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace USBWatcher
{
    public class UDISK
    {
        public string DiskDeviceID { get; set; } 
        public string DiskIndex { get; set; }
        public string Caption { get; set; }
        public MediaType DiskType { get; set; }
        public long TotalSpace { get; set; }

        public List<Partition> Partitions { get; set; }

        public UDISK()
        {

        }

        public UDISK(string diskDeviceID,string diskIndex,string caption, MediaType diskType, long totalSpace)
        {
            this.DiskDeviceID = diskDeviceID;
            this.DiskIndex = diskIndex;
            this.Caption = caption;
            this.DiskType = diskType;
            this.TotalSpace = totalSpace;
        }
    }
}
