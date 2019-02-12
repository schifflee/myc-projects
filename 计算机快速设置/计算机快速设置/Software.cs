using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 计算机快速设置
{
    public class Software
    {
        public string OSName { get; set; }
        public string OSArch { get; set; }
        public string OSVersion { get; set; }
        public List<string> DotNetFrameworkVersions { get; set; }

        public Software()
        {

        }

        public Software(string osname, string osarch, string osversion, List<string> dotnetgrameworkversion)
        {
            this.OSName = osname;
            this.OSArch = osarch;
            this.OSVersion = osversion;
            this.DotNetFrameworkVersions = dotnetgrameworkversion;
        }
    }
}
