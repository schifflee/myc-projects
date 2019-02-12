using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 计算机快速设置
{
    public class ProcessUtil
    {
        public static bool IsProcessRunning(string processname)
        {
            bool flag = false;
            if (Process.GetProcessesByName(processname).ToList().Count>0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        public static void KillProcess(string processname)
        {
            foreach (Process process in Process.GetProcessesByName(processname))
            {

            }
        } 
    }
}
