using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBWatcher
{
    public class Parameters
    {
        public const int WM_DEVICECHANGE = 0x219;//U盘插入后，OS的底层会自动检测到，然后向应用程序发送“硬件设备状态改变“的消息
        public const int DBT_DEVICEARRIVAL = 0x8000;  //就是用来表示U盘可用的。一个设备或媒体已被插入一块，现在可用。
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //一个设备或媒体片已被删除。
        public const uint GENERIC_READ = 0x80000000;
        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int IOCTL_STORAGE_EJECT_MEDIA = 0x2d4808;
        public const string MSG_MONITORING = "开始监控U盘状态！";
        public const string MSG_NEWUDISKINSERTED = "U盘已插入！";
        public const string MSG_UDISKREMOVED = "U盘已拔出！";
        public const string MSG_EMPTYSELECTIONPROMPT = "请至少选择一个U盘！";
        public const string MSG_ERROR1 = "创建U盘快捷方式时出错！";
        public const string MSG_ERROR2 = "U盘未能安全弹出，当前正在被占用！";
        public const string MSG_ERROR3 = "打开U盘时出错！";
    }
}
