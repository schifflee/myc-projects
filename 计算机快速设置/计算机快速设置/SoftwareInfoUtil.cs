using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.Collections;
using System.Diagnostics;

namespace 计算机快速设置
{
    public class SoftwareInfoUtil
    {
        private static ManagementObjectSearcher SystemInfoFetcher(string managementproperty)
        {
            SelectQuery query = new SelectQuery(managementproperty);
            ManagementObjectSearcher mos = new ManagementObjectSearcher(query);
            return mos;
        }

        public static string GetOSName()
        {
            string osname = string.Empty;
            string csdversion = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_OperatingSystem").Get())
            {
                osname = managementobject["Caption"].ToString();
            }
            try
            {
                foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_OperatingSystem").Get())
                {
                    csdversion = managementobject["CSDVersion"].ToString();
                }
            }
            catch (Exception)
            {
                csdversion = string.Empty;
            }
            return string.Format("{0} {1}",osname,csdversion);
        }
        
        public static string GetOSArch()
        {
            string osarch = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_OperatingSystem").Get())
            {
                osarch = managementobject["OSArchitecture"].ToString();
            }
            return osarch;
        }

        public static string GetOSVersion()
        {
            string osversion = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_OperatingSystem").Get())
            {
                osversion = managementobject["Version"].ToString();
            }            
            return osversion;
        }

        public static void RunAppByCMD(string command)
        {
            //Process process = new Process();
            //process.
        }

        public static List<string> GetDotNetFramworkVersions()
        {
            List<string> dotnetframeworkversions = new List<string>();
            string registryitem = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP";
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(registryitem);
            string[] items = key1.GetSubKeyNames();
            ArrayList subkeys = new ArrayList();
            RegistryKey key2;
            foreach (string item in items)
            {
                if (item != "CDF")
                {
                    if (item.Contains("v4"))
                    {
                        if (item != "v4.0")
                        {
                            key2 = key1.OpenSubKey("v4\\Client");
                            dotnetframeworkversions.Add(key2.GetValue("Version", "").ToString());
                        }
                    }
                    key2 = key1.OpenSubKey(item);
                    dotnetframeworkversions.Add(key2.GetValue("Version", "").ToString());
                }
            }
            dotnetframeworkversions = dotnetframeworkversions.Where(x => x != "").ToList();
            return dotnetframeworkversions;
        }
    }
}
