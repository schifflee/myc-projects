using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace USBWatcher
{
    public class ServiceMsgBox
    {
        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        public static void Show(string message, string title)
        {
            int resp = 0;
            WTSSendMessage(
                WTS_CURRENT_SERVER_HANDLE,
                WTSGetActiveConsoleSessionId(),
                title, title.Length,
                message, message.Length,
                0, 0, out resp, false);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WTSGetActiveConsoleSessionId();

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSSendMessage(
            IntPtr hServer,
            int SessionId,
            String pTitle,
            int TitleLength,
            String pMessage,
            int MessageLength,
            int Style,
            int Timeout,
            out int pResponse,
            bool bWait);
    }
}
