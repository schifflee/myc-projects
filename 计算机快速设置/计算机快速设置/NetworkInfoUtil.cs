using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Management;

namespace 计算机快速设置
{
    public class NetworkInfoUtil
    {
        [DllImport("winInet.dll")]
        private extern static bool InternetGetConnectedState(ref int Description, int ReservedValue);

        public static string GetConnectionState()
        {
            string connectionstate = string.Empty;
            int Flag = 0;
            if (!(InternetGetConnectedState(ref Flag, 0)))
            {
                connectionstate = "无 Internet 访问";
            }
            else
            {
                if ((Flag & ConnectionType.INTERNET_CONNECTION_MODEM) != 0)
                {
                    connectionstate += "通过 MODEM 访问 Internet";
                }
                else if ((Flag & ConnectionType.INTERNET_CONNECTION_LAN) != 0)
                {
                    connectionstate += "通过 LAN局域网 访问 Internet";
                }
                else if ((Flag & ConnectionType.INTERNET_CONNECTION_PROXY) != 0)
                {
                    connectionstate += "通过 Internet代理 访问 Internet";
                }
                else if ((Flag & ConnectionType.INTERNET_CONNECTION_MODEM_BUSY) != 0)
                {
                    connectionstate += "可访问 Internet，但 MODEM 被占用";
                }
            }
            return connectionstate;
        }

        public static bool IsNetworkAdapterAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        public static List<NetworkAdapter> GetNetworkAdapters()
        {
            List<NetworkAdapter> adapters = new List<NetworkAdapter>();
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkinterface in nis)
            {
                NetworkAdapter adapter = new NetworkAdapter();
                if (!IsNetworkAdapterAvailable())
                {
                    adapter.WorkCondition = "无可用网卡";
                }
                else
                {
                    if (networkinterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        adapter.Name = networkinterface.Name;
                        adapter.Description = networkinterface.Description;
                        adapter.GUID = networkinterface.Id;
                        PhysicalAddress physicalAddress = networkinterface.GetPhysicalAddress();
                        StringBuilder sb = new StringBuilder();
                        byte[] bytes = physicalAddress.GetAddressBytes();
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            sb.Append(bytes[i].ToString("X2"));
                            if (i != bytes.Length - 1)
                            {
                                sb.Append("-");
                            }
                        }
                        adapter.MacAddress = sb.ToString();
                        IPInterfaceProperties ipproperties = networkinterface.GetIPProperties();
                        GatewayIPAddressInformationCollection gatewaysddresses = ipproperties.GatewayAddresses;
                        foreach (UnicastIPAddressInformation ipaddressinformation in ipproperties.UnicastAddresses)
                        {
                            if (ipaddressinformation.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                adapter.IPV4Address = ipaddressinformation.Address.ToString();
                                adapter.IPV4Mask = ipaddressinformation.IPv4Mask.ToString();
                            }
                        }
                        if (gatewaysddresses.Count > 0)
                        {
                            adapter.GatewayAddress = gatewaysddresses[0].Address.ToString();
                        }
                        else
                        {
                            adapter.GatewayAddress = string.Empty;
                        }
                        if (ipproperties.DhcpServerAddresses.Count == 0)
                        {
                            adapter.DHCPStatus = "关闭";
                        }
                        else
                        {
                            adapter.DHCPStatus = "开启";
                        }
                        for (int i = 0; i < ipproperties.DhcpServerAddresses.Count; i++)
                        {
                            adapter.DHCPServer = ipproperties.DhcpServerAddresses[i].ToString();
                        }
                        for (int i = 0; i < ipproperties.DnsAddresses.Count; i++)
                        {
                            if (ipproperties.DnsAddresses[i].AddressFamily == AddressFamily.InterNetwork)
                            {
                                if (i == 0)
                                {
                                    adapter.FirstDNSServer = ipproperties.DnsAddresses[0].ToString();
                                }
                            }
                        }
                        for (int i = 0; i < ipproperties.DnsAddresses.Count; i++)
                        {
                            if (ipproperties.DnsAddresses[i].AddressFamily == AddressFamily.InterNetwork)
                            {
                                if (i > 0)
                                {
                                    adapter.FirstDNSServer = ipproperties.DnsAddresses[i].ToString();
                                }
                            }
                        }
                        adapters.Add(adapter);
                    }
                }
            }
            return adapters;
        }

        public static bool IsPhysicalNetworkAdapter(NetworkAdapter adapter)
        {
            bool flag = false;
            string registerkey = string.Format("SYSTEM\\CurrentControlSet\\Control\\Network\\{0}\\{1}\\Connection", "{4D36E972-E325-11CE-BFC1-08002BE10318}", adapter.GUID);
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(registerkey, false);
            if (rk != null)
            {
                string PnpInstanceID = rk.GetValue("PnpInstanceId", "").ToString();
                if (PnpInstanceID.Length > 3 && (PnpInstanceID.Contains("PCI") || PnpInstanceID.Contains("USB")))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static void SetNetworkInfo(NetworkAdapter adapter, out string status)
        {
            ManagementObjectCollection moc = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            foreach (ManagementObject mo in moc)
            {
                string IPstatus = "";
                string Gatewaystatus = "";
                string DNSstatus = "";
                if (!(bool)mo["IPEnabled"])
                {
                    continue;
                }
                if (adapter.GUID.Equals(mo["SettingID"].ToString()))
                {
                    if (adapter != null)
                    {
                        inPar = mo.GetMethodParameters("EnableStatic");
                        inPar["IPAddress"] = new string[] { adapter.IPV4Address };
                        inPar["SubnetMask"] = new string[] { adapter.IPV4Mask };
                        outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                        IPstatus = outPar["returnvalue"].ToString();
                        inPar = mo.GetMethodParameters("SetGateways");
                        inPar["DefaultIPGateway"] = new string[] { adapter.GatewayAddress };
                        outPar = mo.InvokeMethod("SetGateways", inPar, null);
                        Gatewaystatus = outPar["returnvalue"].ToString();
                        inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        inPar["DNSServerSearchOrder"] = new string[] { adapter.FirstDNSServer, adapter.BackupDNSServer };
                        outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                        DNSstatus = outPar["returnvalue"].ToString();
                        if (IPstatus == "0" && Gatewaystatus == "0" & DNSstatus == "0")
                        {
                            status = "计算机IP地址设置成功,电脑无需启动！";
                        }
                        else
                        {
                            status = "计算机IP地址设置成功,但需要重新启动！";
                        }
                    }
                }
            }
            status = null;
        }

        public static void EnableDHCP(NetworkAdapter adapter)
        {
            ManagementObjectCollection moc = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;

                if (adapter.GUID.Equals(mo["SettingID"].ToString()))
                {
                    mo.InvokeMethod("SetDNSServerSearchOrder", null);
                    mo.InvokeMethod("EnableDHCP", null);
                }
            }

        }

        public static void EnableNetworkAdapters()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_NetworkAdapter where NetEnabled!=null");
            foreach (ManagementObject mo in mos.Get())
            {
                mo.InvokeMethod("Enable", null);
            }
        }

        public static void DisableNetworkAdapters()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_NetworkAdapter where NetEnabled!=null");
            foreach (ManagementObject mo in mos.Get())
            {
                mo.InvokeMethod("Disable", null);
            }
        }

    }
}
