using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyKTV_Management_Studio
{
    public class ValidationUtil
    {
        private static Regex Reg;
        public static bool IsLegalInput(string text)
        {
            Reg = new Regex(PARAMS.REGEX_NUMBRIC);
            if (Reg.IsMatch(text))
            {
                return true;
            }
            return false;
        }

        public static bool IsLessThanMaxPageCount(int value)
        {
            if (value<=PARAMS.PAGESIZE_MAX)
            {
                return true;
            }
            return false;
        }
    }
}
