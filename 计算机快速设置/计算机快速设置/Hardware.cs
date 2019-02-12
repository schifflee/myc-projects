using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 计算机快速设置
{
    public class Hardware
    {
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string GPU { get; set; }

        public Hardware()
        {

        }

        public Hardware(string cpu, string ram,string gpu)
        {
            this.CPU = cpu;
            this.RAM = ram;
            this.GPU = gpu;
        }
    }
}
