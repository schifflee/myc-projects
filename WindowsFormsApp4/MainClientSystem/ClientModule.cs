using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MainClientSystem
{
    public class ClientModule
    {
        private Socket clientsocket = null;

        private WorkBench workbench;

        public string MSG_Recieve { get; set; }
        public string MSG_Error { get; set; }

        private byte[] buff = new byte[1024 * 1024];
        public void ConnectToServer(string ipAddress, int port)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(GenericUtil.GenerateStr(32));
            workbench = new WorkBench();
            if (workbench.IsConnected(out string msg))
            {
                IPAddress ip = IPAddress.Parse(ipAddress);
                IPEndPoint point = new IPEndPoint(ip, port);
                clientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    clientsocket.Connect(point);
                    Thread thread = new Thread(ReciveFromServer);
                    thread.Start();
                    Thread.Sleep(2000);
                    msg = null;
                }
                catch (SocketException ex)
                {
                    msg = ex.Message;
                    return;
                    throw new SocketException();
                }
                this.MSG_Error = msg;
            }
        }

        public void ReciveFromServer()
        {
            while (true)
            {
                try
                {
                    int msglength = clientsocket.Receive(buff);
                    string str = Encoding.UTF8.GetString(buff, 0, msglength);
                    this.MSG_Recieve = str;
                }
                catch (SocketException ex)
                {
                    this.MSG_Recieve = null;
                    this.MSG_Error = ex.Message;
                    clientsocket.Shutdown(SocketShutdown.Both);
                    clientsocket.Close();
                    break;
                }
            }
        }
    }    
}
