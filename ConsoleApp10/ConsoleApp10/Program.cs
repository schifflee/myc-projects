using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Management;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace ConsoleApp10
{
    class Program
    {
        static Socket socketwatch = null;
        static Dictionary<string, Socket> ClientConnectionItems = new Dictionary<string, Socket>();
        static void Main(string[] args)
        {
            int port = 6000;
            IPAddress ip = IPAddress.Any;
            IPEndPoint ipe = new IPEndPoint(ip, port);
            socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketwatch.Bind(ipe);
            socketwatch.Listen(2);
            Thread threadwatch = new Thread(WatchConnecting)
            {
                IsBackground = true
            };
            threadwatch.Start();
            Console.WriteLine("开启监听......");
            Console.WriteLine("点击输入任意数据回车退出程序......");
            Console.ReadKey();
            socketwatch.Close();
        }

        static void WatchConnecting()
        {
            Socket connection = null;
            //持续不断监听客户端发来的请求     
            while (true)
            {
                try
                {
                    connection = socketwatch.Accept();
                }
                catch (Exception ex)
                {
                    //提示套接字监听异常     
                    Console.WriteLine(ex.Message);
                    break;
                }

                //客户端网络结点号  
                string remoteEndPoint = connection.RemoteEndPoint.ToString();
                //添加客户端信息  
                ClientConnectionItems.Add(remoteEndPoint, connection);
                //显示与客户端连接情况
                Console.WriteLine("\r\n[客户端\"" + remoteEndPoint + "\"建立连接成功！ 客户端数量：" + ClientConnectionItems.Count + "]");

                //获取客户端的IP和端口号  
                IPAddress clientIP = (connection.RemoteEndPoint as IPEndPoint).Address;
                int clientPort = (connection.RemoteEndPoint as IPEndPoint).Port;

                //让客户显示"连接成功的"的信息  
                string sendmsg = "连接服务端成功!";
                byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendmsg);                
                connection.Send(arrSendMsg);
                //创建一个通信线程      
                Thread thread = new Thread(recv)
                {
                    //设置为后台线程，随着主线程退出而退出 
                    IsBackground = true
                };
                //启动线程     
                thread.Start(connection);
            }
        }

        static void recv(object socketclient)
        {
            Socket socketServer = socketclient as Socket;
            while (true)
            {
                byte[] arrServerRecMsg = new byte[1024 * 1024];
                //将接收到的信息存入到内存缓冲区，并返回其字节数组的长度    
                try
                {
                    int length = socketServer.Receive(arrServerRecMsg);

                    //将机器接受到的字节数组转换为人可以读懂的字符串     
                    string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);

                    //将发送的字符串信息附加到文本框txtMsg上     
                    Console.WriteLine("\r\n[客户端：" + socketServer.RemoteEndPoint + " 时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "]\r\n" + strSRecMsg);

                    //Thread.Sleep(3000);
                    //socketServer.Send(Encoding.UTF8.GetBytes("[" + socketServer.RemoteEndPoint + "]："+strSRecMsg));
                    //发送客户端数据
                    if (ClientConnectionItems.Count > 0)
                    {
                        foreach (var socketTemp in ClientConnectionItems)
                        {
                            socketTemp.Value.Send(Encoding.UTF8.GetBytes("[" + socketServer.RemoteEndPoint + "]：" + strSRecMsg));
                        }
                    }
                }
                catch (Exception)
                {
                    ClientConnectionItems.Remove(socketServer.RemoteEndPoint.ToString());
                    //提示套接字监听异常  
                    Console.WriteLine("\r\n[客户端\"" + socketServer.RemoteEndPoint + "\"已经中断连接！ 客户端数量：" + ClientConnectionItems.Count + "]");
                    //关闭之前accept出来的和客户端进行通信的套接字 
                    socketServer.Close();
                    break;
                }
            }
        }
    }
}
