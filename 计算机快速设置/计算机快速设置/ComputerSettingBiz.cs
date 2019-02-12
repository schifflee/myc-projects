using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace 计算机快速设置
{
    public class ComputerSettingBiz
    {
        private static StringBuilder sb;
        public static Computer computer;
        public static NetworkAdapter networkadapter;
        public static Network network;
        public static Software software;
        public static Hardware hardware;
        public static List<NetworkAdapter> networkadapters;
        public static List<NetworkAdapter> physicalnetworkadapters;
        public static List<NetworkAdapter> othernetworkadapters;
        public static string computertype;
        public static string computerbrand;
        public static Image computertypeimage;

        public static void GetComputerInfo()
        {
            computer = new Computer(ComputerInfoUtil.GetHostName(), ComputerInfoUtil.GetDescription(),ComputerInfoUtil.GetDNSHostName(),ComputerInfoUtil.GetUser(), ComputerInfoUtil.GetADStatus(),ComputerInfoUtil.GetDomainName());
            hardware = new Hardware(ComputerInfoUtil.GetCPUInfo(), ComputerInfoUtil.GetRAMInfo(), ComputerInfoUtil.GetGPUInfo());
        }

        public static void GetComputerType()
        {
            switch (ComputerInfoUtil.GetComputerType())
            {
                case 3:
                    computertype = "台式电脑";
                    break;
                case 8:
                    computertype = "平板电脑";
                    break;
                case 9:
                    computertype = "笔记本电脑";
                    break;
                case 13:
                    computertype = "一体机";
                    break;
                default:
                    computertype = "其他电脑类型";
                    break;
            }
        }

        public static void GetComputerTypeImage(ImageList imagelist)
        {
            switch (ComputerInfoUtil.GetComputerType())
            {
                case 3:
                    computertypeimage = imagelist.Images["Desktop"];
                    break;
                case 9:
                    computertypeimage = imagelist.Images["Portable"];
                    break;
                case 13:
                    computertypeimage = imagelist.Images["AllinOne"];
                    break;
                default:
                    computertypeimage = imagelist.Images["Other"];
                    break;
            }
        }
  
        public static void GetComputerBrand()
        {
            computerbrand = ComputerInfoUtil.GetComputerBrand();
        }

        public static void GetSoftwareInfo()
        {
            software = new Software(SoftwareInfoUtil.GetOSName(),SoftwareInfoUtil.GetOSArch(),SoftwareInfoUtil.GetOSVersion(),SoftwareInfoUtil.GetDotNetFramworkVersions());
        }

        public static void GetNetworkStatus()
        {
            network = new Network(NetworkInfoUtil.GetConnectionState());
        }

        public static void GetNetWorkInfo()
        {
            networkadapters = NetworkInfoUtil.GetNetworkAdapters();
        }

        public static void SplitPhysicalNetworkAdapters()
        {
            physicalnetworkadapters = new List<NetworkAdapter>();
            othernetworkadapters = new List<NetworkAdapter>();
            foreach (NetworkAdapter adapter in networkadapters)
            {
                if (NetworkInfoUtil.IsPhysicalNetworkAdapter(adapter))
                {
                    physicalnetworkadapters.Add(adapter);
                }
                else
                {
                    othernetworkadapters.Add(adapter);
                }
            }
        }

        public static void SetHostName(string hostname)
        {
            string result = string.Empty;
            if (ComputerInfoUtil.SetDNSHostName(hostname,out result))
            {
                MessageBox.Show(result, MSG_STRINGS.ERRORTITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(result, MSG_STRINGS.PROMPTTITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void SetText()
        {
            sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToString("yyyyMMddHHmm") + "-Computer Checked Successfully");
            sb.AppendLine(computer.ToString());
            GetNetworkStatus();
            sb.AppendLine(network.ToString());          
            foreach (NetworkAdapter adapter in networkadapters)
            {
                if (!NetworkInfoUtil.IsNetworkAdapterAvailable())
                {
                    sb.AppendLine(adapter.WorkCondition);
                }
                sb.AppendLine(adapter.ToString());
            } 
        }
    }
}