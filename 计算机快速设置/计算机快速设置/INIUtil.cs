using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 计算机快速设置
{
    public class INIUtil
    {
        #region 调用API
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string section, string key, string def, byte[] lpReturnedString, uint size, string filePath); 
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);
        #endregion

        #region 读INI文件
        public static string ReadString(string Section, string Ident, string Default,string iniFilePath)
        {
            byte[] buffer = new byte[65535];
            uint bufferlength = GetPrivateProfileString(Section, Ident, Default, buffer, (uint)buffer.GetUpperBound(0), iniFilePath);
            string s = Encoding.GetEncoding(0).GetString(buffer).TrimEnd('\0');
            return s.Trim();
        }
        #endregion

        #region 根据文件流缓冲获取字符串
        private static void GetStringsFromBuffer(Byte[] Buffer, int bufflength, StringCollection Strings)
        {
            Strings.Clear();
            if (bufflength != 0)
            {
                int start = 0;
                for (int i = 0; i < bufflength; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start).TrimEnd('\0');
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }
        #endregion

        #region 读取Section名称列表
        public static StringCollection ReadSections(StringCollection Sectionlist, string iniFilePath)
        {
            byte[] buffer = new byte[65535];
            uint bufferlength = GetPrivateProfileString(null, null, null, buffer, (uint)buffer.Length, iniFilePath);
            GetStringsFromBuffer(buffer,(int)bufferlength, Sectionlist);
            return Sectionlist;
        }
        #endregion

        #region 获取Section的数量
        public static int GetSectionCount(string iniFilePath)
        {
            int SectionCount = 0;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(32767);
            SectionCount = GetPrivateProfileSectionNames(pReturnedString, 32767, iniFilePath);
            return SectionCount;
        }
        #endregion

        #region 读取指定Section下的Key列表
        public static StringCollection ReadSectionKeys(string Section,StringCollection Idents,string iniFilePath)
        {
            byte[] buffer = new byte[16384];
            uint bufferlength = GetPrivateProfileString(Section, null, null, buffer, (uint)buffer.GetUpperBound(0), iniFilePath);
            GetStringsFromBuffer(buffer, (int)bufferlength, Idents);
            return Idents;
        }
        #endregion

        #region 读取指定Section下的完整字符
        public static StringCollection ReadSectionValues(string Section,NameValueCollection Values,string iniFilePath)
        {
            StringCollection keylist = new StringCollection();
            ReadSectionKeys(Section,keylist,iniFilePath);
            Values.Clear();
            foreach (string key in keylist)
            {
                Values.Add(key,ReadString(Section,key,"",iniFilePath));
            }
            return keylist;
        }
        #endregion

        #region 写Ini文件
        public static void Writestring(string Section, string Key, string Value, string iniFilePath)
        {
            WritePrivateProfileString(Section, Key, Value, iniFilePath);
        }
        #endregion
    }
}
