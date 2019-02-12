using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 计算机快速设置
{
    public class Computer
    {
        public string HostName { get; set; }
        public string Description { get; set; }
        public string DNSHostName { get; set; } 
        public string User { get; set; }        
        public string Password { get; set; }
        public string UserDescription { get; set; }
        public string UserGroup { get; set; }
        public string ADstatus { get; set; }
        public string DomainorWorkgroupName { get; set; }
        public bool IsInAD { get; set; }

        public Computer()
        {

        }

        public Computer(string hostname,string description, string dnshostname, string user,string adstatus, string domainname)
        {
            this.HostName = hostname;
            this.Description = description;
            this.DNSHostName = dnshostname;
            this.User = user;
            this.ADstatus = adstatus;
            this.DomainorWorkgroupName = domainname;
            
        }

        public Computer(string user, string password, string userdescription,string usergroup)
        {
            this.User = user;
            this.Password = password;
            this.UserDescription = userdescription;
            this.UserGroup = usergroup;
        }

        public override string ToString()
        {
            return string.Format("HostName={0}\r\nDiscription={1}\r\nDNSHostName={2}\r\nIsInAD={3}\r\nDomainorWorkgroupName={4}",this.HostName,this.Description,this.DNSHostName, this.IsInAD, this.DomainorWorkgroupName);
        }
    }
}
