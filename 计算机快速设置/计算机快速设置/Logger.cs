using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;

namespace 计算机快速设置
{
    public class Logger
    {
        private void Initailize()
        {
            XmlConfigurator.Configure();
        }
    }
}
