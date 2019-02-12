using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mex13.PinYin;

namespace MyKTV_Management_Studio
{
    public class StringUtil
    {        
        public static string ConvertToPinYin(string chineseword)
        {
            if (chineseword.Contains("赟"))
            {
                return PingYinHelper.ConvertToAllSpell(chineseword).Replace("BIN","YUN");
            }
            return PingYinHelper.ConvertToAllSpell(chineseword);
        }

        public static string ConvertToLastNamePinYin(string chinesename)
        {
            string lastnamepinyin = PingYinHelper.ConvertToEngName(chinesename).LastName;
            if (chinesename.Contains("赟"))
            {
                return lastnamepinyin.Replace("BIN", "YUN");
            }
            return lastnamepinyin;
        }

        public static string ConvertToFirstNamePinYin(string chinesename)
        {
            string lastnamepinyin = PingYinHelper.ConvertToEngName(chinesename).FirstName;
            if (chinesename.Contains("赟"))
            {
                return lastnamepinyin.Replace("BIN", "YUN");
            }
            return lastnamepinyin;
        }
    }
}
