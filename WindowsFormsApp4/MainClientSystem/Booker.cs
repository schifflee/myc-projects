using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainClientSystem
{
    public class Booker
    {
        public string Name { get; set; }
        public string PhoneNum { get; set; }

        public Booker(string name, string phoneNum)
        {
            this.Name = name;
            this.PhoneNum = phoneNum;
        }

        public bool Validate(string name, string phonenum)
        {
            if (StringValidator.IsChineseWord(name) && StringValidator.IsPhoneNum(phonenum))
            {
                return true;
            }
            return false;
        }

        public bool IsPhoneNumExists(string phoneNum)
        {
            bool flag = false;
            Cinema cinema = new Cinema();
            cinema.Load(out List<string> textinfos);
            if (textinfos == null)
            {
                flag = false;
            }
            foreach (string text in textinfos)
            {
                if (text.Contains(PhoneNum))
                {
                    flag = true;
                    break;                   
                }
            }              
            return flag;
        }
    }
}
