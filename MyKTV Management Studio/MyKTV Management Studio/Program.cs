using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace MyKTV_Management_Studio
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LocalizationKeys.LocalizeString += LocalizationKeys_LocalizeString;
            FrmLogin login = new FrmLogin();
            DialogResult dr = login.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Application.Run(new FrmMain());
            }
        }

        private static void LocalizationKeys_LocalizeString(object sender, LocalizeEventArgs e)
        {
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxCancelButton)
            {
                e.LocalizedValue = "取消";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxNoButton)
            {
                e.LocalizedValue = "否";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxOkButton)
            {
                e.LocalizedValue = "确定";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxYesButton)
            {
                e.LocalizedValue = "是";
                e.Handled = true;
            }
            if (e.Key==DevComponents.DotNetBar.LocalizationKeys.MessageBoxCloseButton)
            {
                e.LocalizedValue = "关闭";
                e.Handled = true;
            }
            if (e.Key==DevComponents.DotNetBar.LocalizationKeys.MessageBoxAbortButton)
            {
                e.LocalizedValue = "终止";
                e.Handled = true;
            }
            if (e.Key==DevComponents.DotNetBar.LocalizationKeys.MessageBoxIgnoreButton)
            {
                e.LocalizedValue = "忽略";
                e.Handled = true;
            }
            if (e.Key==DevComponents.DotNetBar.LocalizationKeys.MessageBoxTryAgainButton)
            {
                e.LocalizedValue = "重试";
                e.Handled = true;
            }
            if (e.Key==DevComponents.DotNetBar.LocalizationKeys.MessageBoxHelpButton)
            {
                e.LocalizedValue = "帮助";
                e.Handled = true;
            }
            if (e.Key == DevComponents.DotNetBar.LocalizationKeys.MessageBoxContinueButton)
            {
                e.LocalizedValue = "继续";
                e.Handled = true;
            }
        }

        private static void CreateDATFileAndWriteInfo<T>(string filepath,List<T> list)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                try
                {                   
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, list);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            
        }
    }
}
