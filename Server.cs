using Newtonsoft.Json;
using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Assignment_3
{
    public class Server
    {

        public TcpListener listener;

        public Server(IPAddress localAddr, int port)
        {
            this.listener = new TcpListener(localAddr, port);
            this.listener.Start();
        }

        public void HandleClient(object clientObj)
        {
            var client = clientObj as TcpClient;
            if (client == null) return;

            var stream = client.GetStream();

            try
            {
                Console.WriteLine("Enter try catch");
                var request = this.Read(stream, client.ReceiveBufferSize);
                var response = this.CheckHeader(request);

                this.Send(stream, response);

                stream.Close();
                client.Dispose();
            }
            catch (IOException)
            {
                Console.WriteLine("No request.");
            }
        }

        public void Send(NetworkStream stream, string data)
        {
            var response = Encoding.UTF8.GetBytes(data);
            Console.WriteLine($"Response: {data}");
            stream.Write(response, 0, response.Length);
        }

        public Request Read(NetworkStream stream, int size)
        {
            byte[] buffer = new byte[size];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Request: {JsonConvert.SerializeObject(request)}");
            return JsonConvert.DeserializeObject<Request>(request);
        }

        private string CheckHeader(Request request)
        {
            var response = new Response();
            if (request == null) {
                response.Status = "missing method missing path missing date missing date";
            } else if (request.Method == null) {
                response.Status = "missing method";
            } else {
                response.MethodCheck(ref request);
            }

            return JsonConvert.SerializeObject(response);
        }
    }
}