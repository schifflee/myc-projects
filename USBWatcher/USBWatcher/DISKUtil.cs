using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace USBWatcher
{
    public class DISKUtil
    {
        private static ManagementObjectCollection GetWMICInstances(string wmicname, string wmicproperty, string wmivalue)
        {
            string sql = string.Format("Select * From {0} Where {1}='{2}'", wmicname, wmicproperty, wmivalue);
            SelectQuery sq = new SelectQuery(sql);
            ManagementObjectCollection moc = new ManagementObjectSearcher(sq).Get();
            return moc;
        }

        private static ManagementObjectCollection GetWMICInstances(string wmicname, string interwmicname, string keyproperty,string keyvalue)
        { 
            string sql = string.Format("ASSOCIATORS OF {{0}.{1}='{2}'} WHERE AssocClass ={3} ",interwmicname,keyproperty,keyvalue,wmicname);
            SelectQuery sq = new SelectQuery(sql);
            ManagementObjectCollection moc = new ManagementObjectSearcher(sq).Get();
            return moc;
        }

        private static ManagementObjectCollection GetWMICInstances(string wmicname)
        {
            string sql = string.Format("Select * From {0}", wmicname);
            SelectQuery sq = new SelectQuery(sql);
            ManagementObjectCollection moc = new ManagementObjectSearcher(sq).Get();
            return moc;
        }

        public static List<UDISK> GetUDISKS()
        {
            List<UDISK> udisks = new List<UDISK>();
            UDISK udisk;
            foreach (ManagementObject modisk in GetWMICInstances("Win32_DiskDrive","MediaType","Removable Media"))
            {
                string deviceid = modisk["DeviceID"].ToString();
                foreach (ManagementObject mopartitions in GetWMICInstances("Win32_DiskDrive", "Win32_DiskDriveToDiskPartition", "DeviceID", deviceid))
                {
                    foreach (ManagementObject modisk1 in GetWMICInstances("Win32_DiskPartition", "Win32_DiskDriveToDiskPartition", "DeviceID", mopartitions["DeviceID"].ToString()))
                    {
                        udisk = new UDISK
                        {
                            DiskIndex = modisk["Index"].ToString(),
                            DiskDeviceID = modisk["DeviceID"].ToString(),
                            Caption = modisk["Caption"].ToString(),
                            DiskType = (MediaType)Enum.Parse(typeof(MediaType), modisk["MediaType"].ToString().Replace(" ", ""), true),
                            TotalSpace = (long)modisk["Size"]
                        };
                        udisks.Add(udisk);
                    }
                }                
            }
            foreach (ManagementObject modisk in GetWMICInstances("Win32_DiskDrive", "MediaType", "External hard disk media"))
            {
                string deviceid = modisk["DeviceID"].ToString();
                foreach (ManagementObject mopartitions in GetWMICInstances("Win32_DiskDrive", "Win32_DiskDriveToDiskPartition", "DeviceID", deviceid))
                {
                    foreach (ManagementObject modisk1 in GetWMICInstances("Win32_DiskPartition", "Win32_DiskDriveToDiskPartition", "DeviceID", mopartitions["DeviceID"].ToString()))
                    {
                        udisk = new UDISK
                        {
                            DiskIndex = modisk["Index"].ToString(),
                            DiskDeviceID = modisk["DeviceID"].ToString(),
                            Caption = modisk["Caption"].ToString(),
                            DiskType = (MediaType)Enum.Parse(typeof(MediaType), modisk["MediaType"].ToString().Replace(" ", ""), true),
                            TotalSpace = (long)modisk["Size"]
                        };
                        udisks.Add(udisk);
                    }
                }
            }
            return udisks;
        }
        
        public static List<Partition> GetPartitions(string name)
        {
            
        }

    }
    public enum MediaType
    {
        RemovableMedia,
        Externalharddiskmedia,
        Fixedharddiskmedia
    }
}
