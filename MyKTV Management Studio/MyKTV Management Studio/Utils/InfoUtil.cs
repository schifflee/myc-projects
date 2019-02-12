using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyKTV_Management_Studio
{
    public class InfoUtil
    {
        #region 从文件中获取特定信息存入Dictionary集合
        public static Dictionary<string, MyInfo> GetInfoByFile(string filepath)
        {
            Dictionary<string, MyInfo> infos = new Dictionary<string, MyInfo>();
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                if (fs.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    infos = bf.Deserialize(fs) as Dictionary<string, MyInfo>;
                }
            }
            return infos;
        }
        #endregion

        #region 从特定信息中获取列表
        public static List<string> GetInfos(string infoname, Dictionary<string, MyInfo> infos)
        {
            List<string> infolist = new List<string>();
            foreach (string info in infos[infoname].InfoValues)
            {
                infolist.Add(info);
            }
            return infolist;
        }
        #endregion

        #region 将当前指定信息保存到文件中
        public static void SaveInfoToFile(string infoname, List<string> list, Dictionary<string, MyInfo> infos, string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                try
                {
                    MyInfo obj = new MyInfo
                    {
                        InfoKey = infoname,
                        InfoValues = list.ToArray()
                    };                    
                    infos.Add(infoname, obj);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, infos);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion
    }

    [Serializable]
    public class MyInfo
    {
        public string InfoKey { get; set; }
        public string[] InfoValues { get; set; }        
    }
}
