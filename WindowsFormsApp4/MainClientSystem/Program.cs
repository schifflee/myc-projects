using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace MainClientSystem
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
            Application.Run(new FrmMain());
        }

        public static readonly string TicketSavedFilePath = string.Format("{0}\\SoldTickets.txt", AppDomain.CurrentDomain.BaseDirectory);
        public static readonly string PosterFilesPath = string.Format("{0}\\PosterFiles", AppDomain.CurrentDomain.BaseDirectory);
        public static readonly string XMLFilePath = string.Format("{0}\\ShowList.xml", AppDomain.CurrentDomain.BaseDirectory);
        public static readonly string TMPPath = string.Format("{0}\\Temp", AppDomain.CurrentDomain.BaseDirectory);
        public static string TicketTitle;
        public static int Discount;
        public static float XPosition;
        public static float YPosition;
        public static bool Landscape;
        public static string DefaultPrinterName = GetDefaultPrinterName();
        public static string ShortTimeStr;
        public static string TicketInfo;
        public static PaperSize PaperSize = new PaperSize("Receipt", 220, 220);
        public static Font FontToPrint = new Font("楷体", 12);
        public static int ShowroomRowNum;
        public static int ShowroomColumnNum;
        public const string ChineseWordRegStr = "[\u4e00-\u9fa5]+";
        public const string PhoneNumRegStr = "^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\\d{8}$";
        public const string PosIntegerRegStr = @"^[1-9]\d*$";
        private static string GetDefaultPrinterName()
        {
            ManagementClass mc = new ManagementClass("Win32_Printer");
            ManagementObjectCollection moc = mc.GetInstances();
            string defaultprintername = string.Empty;
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["Default"] == true)
                {
                    defaultprintername = mo["Name"].ToString();
                }
            }
            return defaultprintername;
        }
    }
}