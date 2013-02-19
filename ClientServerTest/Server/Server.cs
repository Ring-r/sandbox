using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientServerTest
{
    class Server : IDisposable
    {
        private readonly Dictionary<IPAddress, ClientData> clientDataList = new Dictionary<IPAddress, ClientData>();

        public bool IsExit = false;

        public void Start()
        {
            TcpListener tcpListener = new TcpListener(10000);
            try
            {
                tcpListener.Start();
                int timeout = 100;
                while (!this.IsExit)
                {
                    if (tcpListener.Pending())
                    {
                        using (TcpClient client = tcpListener.AcceptTcpClient())
                        {
                            using (NetworkStream stream = client.GetStream())
                            {
                                Byte[] bytes = new Byte[256];
                                int length = stream.Read(bytes, 0, bytes.Length);
                                string data = System.Text.Encoding.ASCII.GetString(bytes, 0, length);
                                Console.WriteLine(data);
                                this.CommandRun(data);
                                //byte[] msg = System.Text.Encoding.ASCII.GetBytes (data);
                                //stream.Write (msg, 0, msg.Length);
                                //Console.WriteLine ("Sent: {0}", data);            
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(timeout);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        public void CommandRun(string cmd)
        {
            switch (cmd)
            {
                case "exit":
                    this.IsExit = true;
                    break;
                case "connected":
                    foreach (IPAddress ipAddress in this.clientDataList.Keys)
                    {
                        Console.WriteLine(ipAddress);
                    }
                    break;
                default:
                    if (cmd.StartsWith("connect "))
                    {
                        string param = cmd.Replace("connect ", "");
                        try
                        {
                            IPAddress ipAddress = IPAddress.Parse(param);
                            if (!this.clientDataList.ContainsKey(ipAddress))
                            {
                                this.clientDataList.Add(ipAddress, new ClientData() { IpAddress = ipAddress });
                            }
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc);
                        }
                    }
                    if (cmd.StartsWith("disconnect "))
                    {
                        string param = cmd.Replace("disconnect ", "");
                        try
                        {
                            IPAddress ipAddress = IPAddress.Parse(param);
                            if (!this.clientDataList.ContainsKey(ipAddress))
                            {
                                this.clientDataList.Remove(ipAddress);
                            }
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc);
                        }
                    }
                    break;
            }
        }
    }
}
