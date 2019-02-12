using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MainServerSystem
{
    public class ServerModule
    {
        private Socket clientsocket = null;
        private Socket serversocket;

        public string IP { get; set; }
        public int Port { get; set; }
        public string MSG_Recieve { get; set; }
        public string MSG_Error { get; set; }
        public string MSG_Send { get; set; }
        public string MSG_Show { get; set; }
        public Dictionary<string, Socket> ClientInfos { get; set; }

        public ServerModule(string ip, int port)
        {
            this.IP = ip;
            this.Port = port;
        }

        public bool InitServer(object obj)
        {
            try
            {
                ClientInfos = new Dictionary<string, Socket>();
                serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(this.IP);
                int port = this.Port;
                serversocket.Bind(new IPEndPoint(ip, port));
                serversocket.Listen(10);
                this.MSG_Show = string.Format("启动监听成功，服务器为：{0}。", serversocket.LocalEndPoint.ToString());
                Thread listenthread = new Thread(ListenClientAndAccessControl)
                {
                    IsBackground = true
                };
                listenthread.Start(obj);
                Thread.Sleep(2000);
                return true;
            }
            catch (SocketException ex)
            {
                this.MSG_Error = string.Format("监听启动失败！{0}", ex.Message);
                serversocket.Shutdown(SocketShutdown.Both);
                serversocket.Close();
                return false;
            }            
        }
    
        public void ListenClientAndAccessControl(object obj)
        {
            while (true)
            {
                try
                {
                    clientsocket = serversocket.Accept();
                    this.ClientInfos.Add(clientsocket.RemoteEndPoint.ToString(), clientsocket);
                    this.MSG_Show = string.Format("客户端[{0}]已连接到服务器！", clientsocket.RemoteEndPoint.ToString());
                    this.MSG_Send = "测试内容";
                    clientsocket.Send(Encoding.UTF8.GetBytes(this.MSG_Send));                              
                    Thread recievethread = new Thread(RecieveFromClient)
                    {
                        IsBackground = true
                    };
                    recievethread.Start(clientsocket);
                }
                catch (SocketException ex)
                {
                    this.MSG_Error = ex.Message;
                    break;
                }
            }                    
        }

        public void RecieveFromClient(object clientSocket)
        {
            Socket recievesocket = (Socket)clientSocket;
            while (true)
            {
                byte[] result = new byte[1024 * 1024];
                try
                {        
                    int recievelength = recievesocket.Receive(result);
                    this.MSG_Recieve = Encoding.UTF8.GetString(result, 0, recievelength);
                    this.MSG_Show = string.Format("客户端{0}于{1}发来消息：{2}", recievesocket.RemoteEndPoint.ToString(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), this.MSG_Recieve);
                    foreach (string key in ClientInfos.Keys)
                    {
                        string s = recievesocket.RemoteEndPoint.ToString();
                        if (key != s)
                        {
                            Socket socket = ClientInfos[key];
                            this.MSG_Show=string.Format("向客户端{0}发送消息：", key);
                            socket.Send(Encoding.UTF8.GetBytes(this.MSG_Recieve));
                        }
                    }
                }
                catch (SocketException ex)
                {
                    this.MSG_Error = ex.Message;
                    this.MSG_Show=string.Format("客户端{0}连接中断！",recievesocket.RemoteEndPoint.ToString());
                    ClientInfos.Remove(recievesocket.RemoteEndPoint.ToString());
                    recievesocket.Shutdown(SocketShutdown.Both);
                    recievesocket.Close();
                    break;
                }
            }
        }

        public bool GetClientOnlineStatus(Socket clientsock)
        {
            if (clientsock.Poll(1000,SelectMode.SelectRead))
            {                
                return false;
            }
            return true;
        }
    }
}
