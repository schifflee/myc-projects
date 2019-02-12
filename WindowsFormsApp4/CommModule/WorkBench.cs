using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace MainClientSystem
{
    public class WorkBench
    {
        [DllImport("sensapi.dll")]
        private static extern bool IsNetworkAlive(out int connectionDescription);
        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref IntPtr dwFlag, int dwReserved);

        public string Hostname { get; set; }
        public byte[] Buff_Send { get; set; }
        public string MSG_Recieve { get; set; }
        public int Timeout { get; set; }
        public int TTL { get; set; }
        public bool DontFragment { get; set; }

        public WorkBench()
        {

        }

        public WorkBench(string hostName, byte[] buff_Send, string msg_Recieve, int timeout, int ttl, bool dontFragment)
        {
            this.Hostname = hostName;
            this.Buff_Send = buff_Send;
            this.MSG_Recieve = msg_Recieve;
            this.Timeout = timeout;
            this.TTL = ttl;
            this.DontFragment = dontFragment;
        }

        public bool IsConnected(out string msg)
        {
            bool isNetworkConnected = IsNetworkAlive(out int flags);
            int errcode = Marshal.GetLastWin32Error();
            if (errcode != 0)
            {
                msg = "获取网络状态异常！";
            }
            if (isNetworkConnected && flags == 1)
            {
                IntPtr dwFlag = new IntPtr();
                isNetworkConnected = InternetGetConnectedState(ref dwFlag, 0);
                errcode = Marshal.GetLastWin32Error();
                if (errcode != 0)
                {
                    msg = "获取网络状态异常！";
                }
            }
            msg = null;
            return isNetworkConnected;
        }

        public void TestConnectingToServer(out string msg)
        {
            AutoResetEvent waitevent = new AutoResetEvent(false);
            Ping ping = new Ping();
            ping.PingCompleted += new PingCompletedEventHandler(ping_PingCompleted);
            string s = GenericUtil.GenerateStr(32);
            this.Buff_Send = Encoding.ASCII.GetBytes(s);
            PingOptions options = new PingOptions(this.TTL, this.DontFragment);
            ping.SendAsync(this.Hostname, this.Timeout, this.Buff_Send, options, waitevent);
            waitevent.WaitOne();
            msg = "测试完成";
            this.MSG_Recieve = msg;
        }

        private void ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ((AutoResetEvent)e.UserState).Set();
            }
            else if (e.Error != null)
            {
                this.MSG_Recieve = string.Format("测试失败，错误原因：{0}", e.Error.Message);
                ((AutoResetEvent)e.UserState).Set();
            }
            PingReply reply = e.Reply;
            ShowDetails(reply);
            ((AutoResetEvent)e.UserState).Set();
        }

        private void ShowDetails(PingReply reply)
        {
            StringBuilder sb = new StringBuilder();
            if (reply == null)
            {
                return;
            }
            sb.AppendLine("测试成功，返回的数据如下：");
            sb.AppendLine(string.Format("主机地址：{0}", reply.Address.ToString()));
            sb.AppendLine(string.Format("延迟：{0}", reply.RoundtripTime.ToString()));
            sb.AppendLine(string.Format("生存时间：{0}", reply.Options.Ttl.ToString()));
            sb.AppendLine(string.Format("控制数据分段：", reply.Options.DontFragment));
            this.MSG_Recieve = sb.ToString();
        }
    }

    public class GenericUtil
    {
        public static string GenerateStr(int byteLen)
        {
            byte[] _byte = new byte[4];
            RNGCryptoServiceProvider rngcsprovider = new RNGCryptoServiceProvider();
            rngcsprovider.GetBytes(_byte);
            Random random = new Random(BitConverter.ToInt32(_byte, 0));
            string s = null, str = string.Empty;
            str += "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
            for (int i = 0; i < byteLen; i++)
            {
                s += str.Substring(random.Next(0, str.Length - 1), 1);
            }
            return s;
        }
    }
}
