using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 计算机快速设置
{
    public class NetworkAdapter
    {
        public string WorkCondition { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GUID { get; set; }
        public string MacAddress { get; set; }
        public string IPV4Address { get; set; }
        public string IPV4Mask { get; set; }
        public string GatewayAddress { get; set; }
        public string DHCPStatus { get; set; }
        public string DHCPServer { get; set; }
        public string FirstDNSServer { get; set; }
        public string BackupDNSServer { get; set; }

        public NetworkAdapter()
        {

        }

        public NetworkAdapter(string workcondition, string name, string description, string guid, string macaddress, string ipv4address, string ipv4mask, string gatewayaddress, string dhcpstatus, string dhcpserver, string firstdnsserver, string backupdnsserver)
        {
            this.WorkCondition = workcondition;
            this.Name = name;
            this.Description = description;
            this.GUID = guid;
            this.MacAddress = macaddress;
            this.IPV4Address = ipv4address;
            this.IPV4Mask = ipv4mask;
            this.GatewayAddress = gatewayaddress;
            this.DHCPStatus = dhcpstatus;
            this.DHCPServer = dhcpserver;
            this.FirstDNSServer = firstdnsserver;
            this.BackupDNSServer = backupdnsserver;
        }
      
        public override string ToString()
        {
            return string.Format("Name={0}\r\nDescription={1}\r\nGUID={2}\r\nMacAddress={3}\r\nDHCPStatus={4}\r\nDHCPServer={5}\r\nIPAddress={6}\r\n" +
                "Mask={7}\r\nGateway={8}\r\nDNSA={9}\r\nDNSB={10}", this.Name, this.Description, this.GUID, this.DHCPStatus, this.DHCPServer, this.IPV4Address, this.IPV4Mask, this.GatewayAddress, this.FirstDNSServer, this.BackupDNSServer);
        }
    }
}
