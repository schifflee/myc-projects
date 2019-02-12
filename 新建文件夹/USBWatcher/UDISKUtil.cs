using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using IWshRuntimeLibrary;
using System.IO;
using System.Diagnostics;

namespace USBWatcher
{
    public class UDISKUtil
    {
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
          IntPtr hDevice,
          uint dwIoControlCode,
          IntPtr lpInBuffer,
          uint nInBufferSize,
          IntPtr lpOutBuffer,
          uint nOutBufferSize,
          out uint lpBytesReturned,
          IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesireAccess,
        uint dwShareMode,
        IntPtr SecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile
        );


        private static ManagementClass WMIC(string wmicname)
        {
            ManagementClass mc = new ManagementClass(wmicname);
            return mc;
        }

        public static List<int> GetDISKIndexes()
        {
            List<int> indexlist = new List<int>();
            int index = 0;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        index = Convert.ToInt32(mo1.Properties["DiskIndex"].Value);
                        indexlist.Add(index);
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        index = Convert.ToInt32(mo1.Properties["DiskIndex"].Value);
                        indexlist.Add(index);
                    }
                }
            }
            return new HashSet<int>(indexlist).ToList<int>();
        }

        public UDISK GetUDISK(int diskindex)
        {

        }

        public static int GetPartitionIndex(string letter)
        {
            int partitionindex = 0;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                partitionindex = Convert.ToInt32(mo1.Properties["Index"].Value);
                            }
                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                partitionindex = Convert.ToInt32(mo1.Properties["Index"].Value);
                            }
                        }
                    }
                }
            }
            return partitionindex;
        }

        public static string GetDISKCaption(int diskindex)
        {
            string caption = string.Empty;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementBaseObject mbo in mo.GetRelated("Win32_DiskPartition"))
                    {
                        if (Convert.ToInt32(mbo.Properties["DiskIndex"].Value) == diskindex)
                        {
                            caption = mo.Properties["Caption"].Value.ToString();
                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementBaseObject mbo in mo.GetRelated("Win32_DiskPartition"))
                    {
                        if (Convert.ToInt32(mbo.Properties["DiskIndex"].Value) == diskindex)
                        {
                            caption = mo.Properties["Caption"].Value.ToString();
                        }
                    }
                }
            }
            return caption;
        }

        public static string GetDISKType(int diskindex)
        {            
            string disktype = string.Empty;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementBaseObject mbo in mo.GetRelated("Win32_DiskPartition"))
                    {
                        if (Convert.ToInt32(mbo.Properties["DiskIndex"].Value) == diskindex)
                        {
                            disktype = "移动硬盘";
                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementBaseObject mbo in mo.GetRelated("Win32_DiskPartition"))
                    {
                        if (Convert.ToInt32(mbo.Properties["DiskIndex"].Value) == diskindex)
                        {
                            disktype = "U盘";
                        }
                    }
                }
            }
            return disktype;
        }

        public static List<string> GetLetters(int diskindex)
        {
            List<string> letterlist = new List<string>();
            string letter = null;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        if (Convert.ToInt32(mo1.Properties["DiskIndex"].Value) == diskindex)
                        {
                            foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                            {
                                letter = mbo.Properties["Name"].Value.ToString();
                                letterlist.Add(letter);
                            }

                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        if (Convert.ToInt32(mo1.Properties["DiskIndex"].Value) == diskindex)
                        {
                            foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                            {
                                letter = mbo.Properties["Name"].Value.ToString();
                                letterlist.Add(letter);
                            }
                        }
                    }
                }
            }
            return letterlist;
        }

        public static string GetVolume(string letter)
        {
            string volumelabel = string.Empty;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {                               
                                volumelabel =  mbo.Properties["VolumeName"].Value.ToString();
                            }
                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                volumelabel = mbo.Properties["VolumeName"].Value.ToString();
                            }
                        }
                    }
                }
            }
            return volumelabel;
        }

        public static string GetPartitionFormat(string letter)
        {
            string partitionformat = string.Empty;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                partitionformat = mbo.Properties["FileSystem"].Value.ToString();
                            }
                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                partitionformat = mbo.Properties["FileSystem"].Value.ToString();
                            }

                        }
                    }
                }
            }
            return partitionformat;
        }       

        public static long GetTotalSpace(int diskindex)
        {
            long totalspace = 0;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        if (Convert.ToInt32(mo1.Properties["DiskIndex"].Value) == diskindex)
                        {
                            totalspace = Convert.ToInt64(mo.Properties["Size"].Value);
                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        if (Convert.ToInt32(mo1.Properties["DiskIndex"].Value) == diskindex)
                        {
                            totalspace = Convert.ToInt64(mo.Properties["Size"].Value);
                        }
                    }
                }
            }
            return totalspace;
        }

        public static long GetPartitionTotalSpace(string letter)
        {
            long partitiontotalspace = 0;                                                     
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                partitiontotalspace = Convert.ToInt64(mbo.Properties["Size"].Value);
                            }
                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                partitiontotalspace = Convert.ToInt64(mbo.Properties["Size"].Value);
                            }
                        }
                    }
                }
            }
            return partitiontotalspace;
        }

        public static long GetPartitionFreeSpace(string letter)
        {
            long partitionfreespace = 0;
            foreach (ManagementObject mo in WMIC("Win32_DiskDrive").GetInstances())
            {
                if (mo.Properties["MediaType"].Value.ToString() == "External hard disk media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                partitionfreespace = Convert.ToInt64(mbo.Properties["FreeSpace"].Value);
                            }
                        }
                    }
                }
                if (mo.Properties["MediaType"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementBaseObject mbo in mo1.GetRelated("Win32_LogicalDisk"))
                        {
                            if (mbo.Properties["Caption"].Value.ToString() == letter)
                            {
                                partitionfreespace = Convert.ToInt64(mbo.Properties["FreeSpace"].Value);
                            }
                        }
                    }
                }
            }
            return partitionfreespace;
        }

     
        //public static bool CreateShortCutofUDISK(UDISK udisk, out string msg)
        //{
        //    string desktoppath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //    WshShell shell = new WshShell();
        //    try
        //    {
        //        IWshShortcut shortcut = null;
        //        foreach (string letter in udisk.Letters)
        //        {
        //            shortcut = (IWshShortcut)shell.CreateShortcut(string.Format(@"{0}\{1}({2})-快捷方式.lnk", desktoppath, GetVolumeLabel(letter),letter.Remove(1)));
        //            shortcut.WorkingDirectory = letter+@"\";
        //            shortcut.WindowStyle = 1;
        //            shortcut.Save();
        //        }
        //        msg = null;
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        msg = Parameters.MSG_ERROR1;
        //        return false;
        //    }
        //}

        //public static bool OpenUDISK(UDISK udisk,out string msg)
        //{
        //    try
        //    {
        //        foreach (string letter in udisk.Letters)
        //        {
        //            Process.Start("explorer", letter);
        //        }
        //        msg = null;
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        msg = Parameters.MSG_ERROR3;
        //        return false;
        //    }
        //}

        //public static bool DeleteUDISKSafety(UDISK udisk)
        //{
        //    bool result = false;
        //    string[] pathnames = null;
        //    for (int i = 0; i < udisk.Letters.Count; i++)
        //    {
        //        pathnames[i] = udisk.Letters[i];
        //    }
        //    foreach (string pathname in pathnames)
        //    {
        //        //IntPtr handle=CreateFile(pathname,Gen)
        //    }
        //    return result;
        //}
    }
}
