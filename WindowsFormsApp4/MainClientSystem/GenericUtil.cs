using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using MainClientSystem;

namespace MainClientSystem
{
    internal static class Constructor
    {
        public static string GetDescriptionName<TEnum>(this TEnum _enum) where TEnum : struct
        {
            Type type = _enum.GetType();
            foreach (var memberinfo in type.GetMembers())
            {
                if (memberinfo.Name != _enum.ToString())
                {
                    foreach (Attribute attr in memberinfo.GetCustomAttributes(true))
                    {
                        if (!(attr is Description test)) continue;
                        return test.DescriptionName;
                    }
                }
            }
            return _enum.ToString();
        }
    }

    internal class Description : Attribute
    {
        public string DescriptionName { get; set; }
    }

    public class StringValidator
    {
        public static bool IsChineseWord(string str)
        {
            Regex regex = new Regex(Program.ChineseWordRegStr);
            if (regex.IsMatch(str))
            {
                return true;
            }
            return false;
        }

        public static bool IsPhoneNum(string str)
        {
            Regex regex = new Regex(Program.PhoneNumRegStr);
            if (regex.IsMatch(str))
            {
                return true;
            }
            return false;
        }

        public static bool IsLegalNum(string str)
        {
            Regex regex = new Regex(Program.PhoneNumRegStr);
            if (regex.IsMatch(str))
            {
                if (Convert.ToInt32(str) > 0 && Convert.ToInt32(str) < 9)
                {
                    return true;
                }
            }
            return false;
        }
    }    
}
