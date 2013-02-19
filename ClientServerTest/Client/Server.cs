using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientServerTest
{
    class Server
    {
        private Thread thread = null;
        private readonly Object threadLock = new Object();

        private readonly Dictionary<IPAddress, ClientData> clientDataList = new Dictionary<IPAddress, ClientData>();

        public bool IsExit = false;

        private void Start()
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
                                //data = this.CommandRun(data);
                                //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                                //stream.Write(msg, 0, msg.Length);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(timeout);
                    }
                }
            }
            catch (SocketException exc)
            {
                Console.WriteLine("SocketException: {0}", exc);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception: {0}", exc);
            }
        }

        public void StartAsync()
        {
            lock (this.threadLock)
            {
                if (this.thread == null)
                {
                    this.thread = new Thread(new ThreadStart(() => this.Start()));
                    this.thread.Start();
                }
            }
        }

        public void StopAsync()
        {
            lock (this.threadLock)
            {
                if (this.thread != null)
                {
                    this.IsExit = true;
                    this.thread.Join();
                    this.thread = null;
                }
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
