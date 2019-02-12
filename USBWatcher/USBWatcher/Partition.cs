using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBWatcher
{
    public class Partition
    {
        public string PartitionDeviceID { get; set; }
        public string PartitionIndex { get; set; }
        public string Letter { get; set; }
        public string Volume { get; set; }
        public string FileSystem { get; set; }
        public long TotalSpace { get; set; }
        public long FreeSpace { get; set; }

        public Partition()
        {

        }

        public Partition(string partitionDeviceID, string partitionIndex, string letter, string volume, string fileSystem, long totalSpace, long freeSpace)
        {
            this.PartitionDeviceID = partitionDeviceID;
            this.PartitionIndex = partitionIndex;
            this.Letter = letter;
            this.Volume = volume;
            this.FileSystem = fileSystem;
            this.TotalSpace = totalSpace;
            this.FreeSpace = freeSpace;
        }
    }
}
