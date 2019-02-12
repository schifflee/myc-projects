using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 计算机快速设置
{
    public class Network
    {
        public string ConnectionState { get; set; }
        public Network(string connectionstate)
        {
            this.ConnectionState = connectionstate;
        }

        public override string ToString()
        {
            return string.Format("ConnectionState={0}",this.ConnectionState);
        }
    }
}
