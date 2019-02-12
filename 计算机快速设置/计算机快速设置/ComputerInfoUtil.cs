using System;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Security.Principal;

namespace 计算机快速设置
{
    public class ComputerInfoUtil
    {
        [DllImport("kernel32.dll", EntryPoint = "SetComputerNameEx", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static int SetComputerNameEx(int iType, string lpComputerName);
        private static ManagementObjectSearcher SystemInfoFetcher(string managementproperty)
        {
            SelectQuery query = new SelectQuery(managementproperty);
            ManagementObjectSearcher mos = new ManagementObjectSearcher(query);
            return mos;
        }
        private static string WinNT = Properties.Resources.WinNTProtocol;
        private static string LDAP = Properties.Resources.LDAPProtocol;

        public static string GetHostName()
        {
            string dnshostname = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_ComputerSystem").Get())
            {
                dnshostname = managementobject["Name"].ToString();
            }
            return dnshostname;
        }

        public static string GetDescription()
        {
            string description = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_ComputerSystem").Get())
            {
                description = managementobject["Description"].ToString();
            }
            return description;
        }

        public static string GetDNSHostName()
        {
            string dnshostname = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_ComputerSystem").Get())
            {
                dnshostname = managementobject["DNSHostName"].ToString();
            }
            return dnshostname;
        }

        public static string GetDomainName()
        {
            string domainname = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_ComputerSystem").Get())
            {
                if (IsInAD())
                {
                    domainname = managementobject["Domain"].ToString();
                }
                else
                {
                    domainname = managementobject["Workgroup"].ToString();
                }
            }
            return domainname;
        }

        public static string GetUser()
        {
            string user = string.Empty;
            try
            {
                foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_ComputerSystem").Get())
                {
                    user = managementobject["UserName"].ToString();
                }
            }
            catch (Exception)
            {
                user = "远程登录用户或其他用户";
            }
            return user;
        }

        private static bool IsInAD()
        {
            bool flag = false;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_ComputerSystem").Get())
            {
                if ((bool)managementobject["PartOfDomain"] == false)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static string GetADStatus()
        {
            if (IsInAD() == false)
            {
                return "否";
            }
            else
            {
                return "是";
            }
        }

        public static string GetNetworkmsg()
        {
            string networkmsg = string.Empty;
            if (IsInAD() == false)
            {
                networkmsg = "工作组 ：";
                foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_ComputerSystem").Get())
                {
                    networkmsg = networkmsg + managementobject["Workgroup"].ToString();
                }
            }
            else
            {
                networkmsg = "域 ：";
                foreach (ManagementObject ManagementObject in SystemInfoFetcher("Win32_ComputerSystem").Get())
                {
                    networkmsg = networkmsg + ManagementObject["Domain"].ToString();
                }
            }
            return networkmsg;
        }

        public static string GetCPUInfo()
        {
            string cpuinfo = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_Processor").Get())
            {
                cpuinfo = managementobject["Name"].ToString();
            }
            return cpuinfo;
        }

        public static string GetRAMInfo()
        {
            double ramvolume = 0;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_PhysicalMemory").Get())
            {
                ramvolume += ((Math.Round(Int64.Parse(managementobject.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024.0, 1)));
            }
            return string.Format("{0}G",ramvolume.ToString());
        } 

        public static string GetGPUInfo()
        {
            string gpuinfo = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_VideoController").Get())
            {
                gpuinfo = managementobject["Name"].ToString();
            }
            return gpuinfo;
        }

        public static bool SetDNSHostName(string dnshostname, out string status)
        {
            bool flag = false;
            Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
            int result = SetComputerNameEx(5, dnshostname);
            if (result == 0)
            {
                status = string.Format("{0}{1}",Properties.Resources.ErrorString2,ex.Message);
                flag = false;
            }
            else
            {
                status = null;
                flag = true;
            }
            return flag;
        }

        public static bool SetDomainName(string domainname, out string status)
        {
            bool flag = false;
            Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
            int result = SetComputerNameEx(6, domainname);
            if (result == 0)
            {
                status = string.Format("{0}{1}", Properties.Resources.ErrorString3, ex.Message);
                flag = false;
            }
            else
            {
                status = null;
                flag = true;
            }
            return flag;
        }

        public static bool SetWindowsUser(string username, string password, string userdescription, string usergroup,out string status)
        {
            bool flag = false;
            DirectoryEntry DIR = new DirectoryEntry(string.Format("{0}{1}", Properties.Resources.WinNTProtocol, Environment.MachineName));
            DirectoryEntry user = DIR.Children.Add(username, "User");
            try
            {
                user.Properties["FullName"].Add(username);
                user.Invoke("SetPassword", password);
                user.Invoke("Put", "Description", userdescription);
                user.Properties["pwdLastSet"].Value = -1;
                user.Invoke("Put", "UserFlags", 66049);
                user.Invoke("Put", "UserFlags", 0x0040);
                user.CommitChanges();
                using (DirectoryEntry group = DIR.Children.Find(usergroup,"group"))
                {
                    if (group.Name!=null)
                    {
                        group.Invoke("Add", user.Path.ToString());
                    }
                }
                status = null;
                flag = true;
            }
            catch (ManagementException ex)
            {
                status = string.Format("{0}{1}", Properties.Resources.ErrorString5, ex.Message);
                flag = false;
            }
            return flag;
        }

        public static int JoinintoWorkgroup(string workgroupname,string username,string password,out string status)
        {
            int flag = 0;
            if (!IsInAD())
            {
                ManagementObject managementobject = new ManagementObject(new ManagementPath(string.Format("Win32_ComputerSystem.Name='{0}'", GetHostName())));
                try
                {
                    ManagementBaseObject inPar = managementobject.GetMethodParameters("JoinDomainOrWorkgroup");
                    inPar["Name"] = workgroupname;
                    inPar["Password"] = password;
                    inPar["UserName"] = username;
                    inPar["AccountOU"] = null;
                    inPar["FJoinOptions"] = 0;
                    ManagementBaseObject outPar = managementobject.InvokeMethod("JoinDomainOrWorkgroup", inPar, null);
                    if (outPar["ReturnValue"].ToString().Contains("Success"))
                    {
                        status = null;
                        flag = 1;
                    }
                    else
                    {
                        status = Properties.Resources.ErrorString6;
                        flag = 0;
                    }
                }
                catch (ManagementException e)
                {
                    status = Properties.Resources.ErrorStringUnknown;
                    flag = -1;
                }
            }
            status = null;
            return flag;
        }

        public static int JoinintoAD(string workgroupname, string username, string password, out string status)
        {
            int flag = 0;
            if (IsInAD())
            {
                ManagementObject managementobject = new ManagementObject(new ManagementPath(string.Format("Win32_ComputerSystem.Name='{0}'", GetHostName())));
                try
                {
                    ManagementBaseObject inPar = managementobject.GetMethodParameters("JoinDomainOrWorkgroup");
                    inPar["Name"] = workgroupname;
                    inPar["Password"] = password;
                    inPar["UserName"] = username;
                    inPar["AccountOU"] = null;
                    inPar["FJoinOptions"] = 3;
                    ManagementBaseObject outPar = managementobject.InvokeMethod("JoinDomainOrWorkgroup", inPar, null);
                    if (outPar["ReturnValue"].ToString().Contains("Success"))
                    {
                        status = null;
                        flag = 1;
                    }
                    else
                    {
                        status = Properties.Resources.ErrorString6;
                        flag = 0;
                    }
                }
                catch (ManagementException e)
                {
                    status = Properties.Resources.ErrorStringUnknown;
                    flag = -1;
                }
            }
            status = null;
            return flag;
        }

        public static bool SyncintoAD(string domainname, string username, string password, DirectoryEntry domain, out string status)
        {
            bool flag = false;
            domain = new DirectoryEntry();
            try
            {
                domain.Path = string.Format("{0}{1}", LDAP, domainname);
                domain.Username = username;
                domain.Password = password;
                domain.AuthenticationType = AuthenticationTypes.Secure;
                domain.RefreshCache();
                status = string.Format("{0}：{1}",Properties.Resources.LDAPProtocol,domainname);
                flag = true;
            }
            catch (Exception ex)
            {
                status = string.Format("{0}{1}",Properties.Resources.ErrorString7,ex.Message);
                flag = false;
            }
            return flag;
        }

        public static int GetComputerType()
        {
            int type = 0;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_SystemEnclosure").Get())
            {
                foreach (int i in (UInt16[])managementobject["ChassisTypes"])
                {
                    if (i > 0 && i < 25)
                    {
                        type = i;
                    }
                }
            }
            return type;
        }

        public static string GetComputerBrand()
        {
            string computerbrand = string.Empty;
            foreach (ManagementObject managementobject in SystemInfoFetcher("Win32_ComputerSystem").Get())
            {
                computerbrand= managementobject["Manufacturer"].ToString();
            }
            return computerbrand;
        }
    }
}
