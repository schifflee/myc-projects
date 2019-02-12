using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace 计算机快速设置
{
    public class ValidationUtil
    {
        private static Regex GetRegex(string regexstr)
        {
            return new Regex(regexstr);
        }

        private static bool IsMatchRegex(string str,Regex regex)
        {
            bool flag = false;
            if (regex.IsMatch(str))
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

    }
}
