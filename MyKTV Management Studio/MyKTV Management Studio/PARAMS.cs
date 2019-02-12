using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyKTV_Management_Studio
{
    public class PARAMS
    {
        public const string ERRORMSG_NULL_UIDFIELD = "管理员用户名不能为空！";
        public const string ERRORMSG_NULL_PWDFIELD = "管理员密码不能为空！";
        public const string ERRORMSG_MISMATCH_UID_PWD = "管理员用户名或密码错误！";
        public const string ERRORMSG_NULL_UID = "管理员用户名不存在！";
        public const string ERRORMSG_LOGINERROR = "登录错误：无法登录，请重试！";
        public const string ERRORMSG_LOGOUTERROR = "登录错误：无法退出登录，请重试！";
        public const string MSG_WINDOWCLOSINGCONFIRM = "是否退出程序？点击“是”关闭窗口，退出程序；点击“否”切换管理员。";
        public const string ERRORMSG_INPUT1 = "输入错误，请输入一个不大于分页总数且大于0的整数！";
        public const string ERRORMSG_INPUT2 = "输入错误，请输入一个大于等于10，小于等于30的整数！";
        public const string TITLE_WINDOWCLOSINGCONFIRM = "窗口关闭确认";
        public const string TITLE_ERROR = "错误";
        public const string TITLE_PROMPT = "提示";
        public static string[] BINFilePath = { AppDomain.CurrentDomain.BaseDirectory + @"\Data\User.bin", AppDomain.CurrentDomain.BaseDirectory + @"\Data\CustomizedInfo.bin" };
        public static string ImageFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Data\Skins";
        public static string DATFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Data\FileCHK.dat";
        public static string AdminName;
        public static int DataBaseStatus;
        public static DateTime DBUpdateDate;
        public const string REGEX_NUMBRIC = @"^[1-9]\d*$";
        public const int PAGESIZE_MAX = 30;
        public const int PAGESIZE_MIN = 10;
        public static int SongListStatus;
        public static bool HasSongsFiltered;       
    }
}
