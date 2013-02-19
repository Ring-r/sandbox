using System;

namespace ClientServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin.");
            Server server = new Server();
            server.Start();
            Console.WriteLine("End.");
        }
    }
}
