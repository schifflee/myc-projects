using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Management;

namespace netbench
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (GetHostType(args[0]))
                {
                    case HostType.LocalHost:
                    case HostType.HostName:
                    case HostType.IPV4Address:
                    case HostType.IPV6Address:
                        Thread th = new Thread(new ParameterizedThreadStart(AutomaticTestHost))
                        {
                            IsBackground = true
                        };
                        th.Start(args[0]);
                        Console.Read();
                        Environment.Exit(0);
                        break;
                }
            }
            else
            {
                Init();
            }
        }

        private static void Init()
        {
            Console.Write("请输入主机位置（IP地址或域名）：");
            string host = Console.ReadLine();
            bool flag = true;
            do
            {
                if (string.IsNullOrEmpty(host))
                {
                    Console.WriteLine("IP地址或域名不能为空！");
                    Console.Write("请输入主机位置（IP地址或域名）：");
                    host = Console.ReadLine();
                }
                else
                {
                    flag = false;
                }
            } while (flag);
            Thread th = new Thread(new ParameterizedThreadStart(ManualTestHost))
            {
                IsBackground = true
            };
            th.Start(host);
            Console.Read();
            Environment.Exit(0);
        }

        private static void AutomaticTestHost(object host)
        {
            Ping ping = new Ping();
            HostType hosttype = GetHostType((string)host);
            switch (hosttype)
            {
                case HostType.LocalHost:
                    Console.WriteLine("LocalHost无需检测！");
                    break;
                case HostType.HostName:
                case HostType.IPV4Address:
                case HostType.IPV6Address:
                    try
                    {
                        PingReply reply = ping.Send((string)host, 5000);
                        if (reply.Status == IPStatus.Success)
                        {
                            Console.Write("连接成功！");
                            Console.WriteLine("返回时间：{0}ms", reply.RoundtripTime);
                        }
                        else
                        {
                            Console.WriteLine("连接失败!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0}：{1}", ex.Message, ex.InnerException.Message);
                    }
                    break;                   
            }
            Console.Write("按任意键退出...");
        }

        private static void ManualTestHost(object host)
        {
            Ping ping = new Ping();
            HostType hosttype=GetHostType((string)host);
            switch (hosttype)
            {
                case HostType.LocalHost:
                    Console.WriteLine("LocalHost无需检测！");
                    break;
                case HostType.HostName:
                case HostType.IPV4Address:
                case HostType.IPV6Address:
                    try
                    {
                        PingReply reply = ping.Send((string)host, 5000);
                        if (reply.Status == IPStatus.Success)
                        {
                            Console.Write("连接成功！");
                            Console.WriteLine("返回时间：{0}ms", reply.RoundtripTime);
                        }
                        else
                        {
                            Console.WriteLine("连接失败!");
                        }
                    }
                    catch (Exception ex)
                    {                       
                        Console.WriteLine("{0}：{1}",ex.Message,ex.InnerException.Message);
                    }
                    break;
            }
            Console.Write("按任意键退出...");
        }

        private static HostType GetHostType(string host)
        {
            IPAddress[] iplist = Dns.GetHostEntry(Dns.GetHostName()).AddressList;           
            if (host.Equals("172.0.0.1")||GetPhysicalAdapterIPList().ToList<string>().Contains(host)
                ||host.Equals(".")||host.Contains(@"(local)\")
                ||host.Contains(Dns.GetHostName().ToUpper()+@"\")
                ||host.Equals("localhost",StringComparison.OrdinalIgnoreCase)
                ||host.Equals("::1"))
            {
                 return HostType.LocalHost;
            }
            else if (new Regex(RegexString.REGEX_IPV4ADDRESS).IsMatch(host))
            {
                 return HostType.IPV4Address;
            }
            else if (new Regex(RegexString.REGEX_IPV6ADDRESS).IsMatch(host))
            {
                 return HostType.IPV6Address;
            }
            return HostType.HostName;
        }

        private static IEnumerable<string> GetPhysicalAdapterIPList()
        {
            foreach (ManagementObject mo in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
            {
                string servicename = mo["ServiceName"].ToString();
                if (!(bool)mo["IPEnabled"])
                { continue; }
                if (servicename.ToLower().Contains("vmnetadapter")
               || servicename.ToLower().Contains("ppoe")
               || servicename.ToLower().Contains("bthpan")
               || servicename.ToLower().Contains("tapvpn")
               || servicename.ToLower().Contains("ndisip")
               || servicename.ToLower().Contains("sinforvnic")
               || servicename.ToLower().Contains("vboxnetadp"))
                { continue; }
                if (mo["IPAddress"] is string[] ipaddr)
                {
                    foreach (var item in ipaddr)
                    {
                        if (item != "0.0.0.0")
                        {
                            yield return item;
                        }
                    }
                }
            }
        }
    }

    public enum HostType
    {
        LocalHost = 1,
        HostName = 2,
        IPV4Address = 3,
        IPV6Address= 4,
    }

    internal class RegexString
    {
        public const string REGEX_IPV4ADDRESS = @"(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)";
        public const string REGEX_IPV6ADDRESS = @"^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d))
                                                {3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}
                                                (((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]
                                                {1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|
                                                ((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|
                                                ((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|
                                                2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$";
    }
}
