using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyKTV_Management_Studio
{
    public class AdminInfoUtil
    {

        #region 从文件中获取管理员信息存入Dictionary集合
        public static Dictionary<string, Administrator> GetAdminInfoByFile(string filepath)
        {
            Dictionary<string, Administrator> admininfo = new Dictionary<string, Administrator>();
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                if (fs.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    admininfo = bf.Deserialize(fs) as Dictionary<string, Administrator>;
                }
            }
            return admininfo;
        }
        #endregion

        #region 从Dictionary类型的管理员信息中获取管理员用户名列表
        public static List<string> GetAdminNames(Dictionary<string, Administrator> admininfo)
        {
            List<string> adminnamelist = new List<string>();
            foreach (Administrator admin in admininfo.Values)
            {
                adminnamelist.Add(admin.Username);
            }
            return adminnamelist;
        }
        #endregion

        #region 根据管理员信息的用户名列表中的指定用户名获取密码
        public static string GetPwdByAdminName(string adminname, Dictionary<string, Administrator> admininfo)
        {
            string pwd = string.Empty;
            for (int i = 0; i < admininfo.Count; i++)
            {
                if (admininfo.ContainsKey(adminname))
                {
                    pwd = admininfo[adminname].Password;
                }
            }
            return pwd;
        }
        #endregion

        #region 将当前指定管理员信息保存到文件中
        public static void SaveInfoToFile(string adminname, string pwd, Dictionary<string, Administrator> admininfo, string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                try
                {
                    Administrator admin = new Administrator
                    {
                        Username = adminname,
                        Password = pwd
                    };
                    admininfo.Add(adminname, admin);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, admininfo);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion
    }
}
