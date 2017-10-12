using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Assignment_3
{
    class Program
    {
        static void Main()
        {
            int port = 5000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            var server = new Server(localAddr, port);

            Console.WriteLine("Server has started !");

            while (true) {
                var client = server.listener.AcceptTcpClient();

                Console.WriteLine("Client connected !");

                var thread = new Thread(server.HandleClient);
                thread.Start(client);
            }
        }
    }
}
