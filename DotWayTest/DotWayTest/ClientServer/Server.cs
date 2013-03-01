using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DotWayTest
{
    class Server
    {
        private Thread thread = null;
        private readonly Object threadLock = new Object();

        public Map map = null;
        private readonly Dictionary<string, ClientData> clientDataList = new Dictionary<string, ClientData>();

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
                                string data = Encoding.ASCII.GetString(bytes, 0, length);
                                Console.WriteLine(data);
                                switch (data)
                                {
                                    case "connect":
                                        string clientAddress = client.Client.RemoteEndPoint.ToString();
                                        if (!this.clientDataList.ContainsKey(clientAddress))
                                        {
                                            this.clientDataList.Add(clientAddress, new ClientData() { Addres = clientAddress });
                                            // TODO: Send all information (map points, milkyMen information (stacks, location, speed)) to client by using stream.Write();
                                        }
                                        break;
                                }
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

        public void GetConnect(Client client)
        {
            this.clientList.Add(client);
            this.SendMapTo(client, this.map);
        }

        public void StartGame()
        {
            Options.DotsCount = Options.random.Next(5, 10);
            this.map.DotsInit();
            this.SendMapToAll(this.map);
        }


        private void SendMapTo(Client client, Map map)
        {
            client.GetMap(map);
        }

        private void SendMapToAll(Map map)
        {
            foreach (Client client in this.clientList)
            {
                this.SendMapTo(client, map);
            }
        }

        public void GetMilkyMan(MilkyMan milkyMan)
        {
            this.map.milkyManList.Add(milkyMan);
        }
    }
}
