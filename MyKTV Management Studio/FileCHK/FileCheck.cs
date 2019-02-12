using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace FileCHK
{
    public class FileCheck
    {
        public List<string> GetFilesAndDirs(string path)
        {
            List<string> fileanddirlist = new List<string>();
            FileAttributes attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                if (Directory.GetFiles(path).Length != 0 || Directory.GetDirectories(path).Length != 0)
                {
                    foreach (string filepath in Directory.GetFiles(path))
                    {
                        fileanddirlist.Add(filepath);
                    }
                    foreach (string dir in Directory.GetDirectories(path))
                    {
                        fileanddirlist.Add(dir);
                    }
                    return fileanddirlist;
                }
                return null;
            }
            return null;
        }

        public bool IsDirEmpty(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                if (Directory.GetFiles(path).Length != 0 && Directory.GetDirectories(path).Length != 0)
                {
                    return true;
                }               
            }
            return false;
        }

        public bool CompareFileAndDir(List<string> fileanddirlist, string path)
        {
            return false;
        }
    }
}
