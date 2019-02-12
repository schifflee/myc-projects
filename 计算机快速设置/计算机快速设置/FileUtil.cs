using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace 计算机快速设置
{
    public class FileUtil
    {
        public static void ExporttoTXT(string text,string filepath)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            using (FileStream fs=new FileStream(filepath,FileMode.Append))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public static void ExporttoINI(string text, string filepath)
        {
               
        }
    }
}