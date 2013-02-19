using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientServerTest
{
    class Client
    {
        private string hostname = null;
        private int port = 10000;
        private void SendMessage(string message)
        {
            try
            {
                using (TcpClient client = new TcpClient(hostname, port))
                {
                    Byte[] data = Encoding.ASCII.GetBytes(message);
                    using (NetworkStream stream = client.GetStream())
                    {
                        stream.Write(data, 0, data.Length);
                        data = new Byte[256];
                        //String responseData = String.Empty;
                        //Int32 bytes = stream.Read(data, 0, data.Length);
                        //responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public void Connect(string hostname)
        {
            // TODO: Send message "connect".
            // TODO: Get responce.
            // TODO: If all right then
            {
                this.hostname = hostname;
            }
        }
        public void Disconnect()
        {
            // TODO: Send message "disconnect".
            // TODO: Get responce.
            // TODO: If all right then
            {
                this.hostname = null;
            }
        }

        public void Listen()
        {
            // TODO: Start listening.
            // TODO: Switch. Wait first ring. Wait second ring. Wait other command.
        }

        public void EndPath()
        {
            // TODO: Calculate remainig path by Greedy Algorithm.
        }
        public void SendPath()
        {
            // TODO: Convert path to json.
            // TODO: Send message.
        }

        public void CommandRun(string cmd)
        {
        }
    }
}
