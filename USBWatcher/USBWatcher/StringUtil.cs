using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace USBWatcher
{
    public class StringUtil
    {
        public static string ToGigaByteFormat(long _byte)
        {
            return string.Format("{0}GB", (Convert.ToDouble(_byte) / (1024 * 1024 * 1024)).ToString("f2"));
        }

        public static string ToMegaByteFormat(long _byte)
        {
            return string.Format("{0}MB", ((int)(Convert.ToDouble(_byte) / (1024 * 1024 * 1024))).ToString());
        }


        public static double ToGigaByte(long _byte)
        {
            return Convert.ToDouble(_byte) / (1024 * 1024 * 1024);
        }

        public static double ToMegaByte(long _byte)
        {
            return Convert.ToDouble(_byte) / (1024 * 1024);
        }

        public static string GetDiskType(MediaType mediatype)
        {
            string disktype = string.Empty;
            switch (mediatype)
            {
                case MediaType.RemovableMedia:
                    disktype = "U盘";
                    break;
                case MediaType.Externalharddiskmedia:
                    disktype = "移动硬盘";
                    break;
                case MediaType.Fixedharddiskmedia:
                    disktype = "本地硬盘";
                    break;
            }
            return disktype;
        }
    }
}
