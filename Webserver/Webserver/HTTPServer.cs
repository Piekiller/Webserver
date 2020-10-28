using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace Webserver
{
    public class HTTPServer
    {
        TcpListener server;

        public HTTPServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            server.Start();
            List<Task> clients = new List<Task>();
            while (true)
            {
                Task clienttask = new Task(HandleClient);
                clienttask.Start();
                clients.Add(clienttask);
            }
        }
        private void HandleClient()
        {
            using var client = server.AcceptTcpClient();
            Console.WriteLine("Client connected");
            using StreamReader sr = new StreamReader(client.GetStream());
            using StreamWriter sw = new StreamWriter(client.GetStream());
            string message = null;
            while (true)
            {
                while ((message = sr.ReadLine()) != "")
                {
                    // Translate data bytes to a ASCII string.
                    message = sr.ReadLine();
                    Console.WriteLine(message);
                    // Process the data sent by the client.
                }
                SendSuccess(sw);
            }
            
        }
        private void SendSuccess(StreamWriter stream)
        {
            string message = "test";
            string response = $"HTTP/1.1 200 OK\nContent-Length: {message.Length}\nContent-Type: text/plain; charset=utf-8\n\n{message}";
            Console.WriteLine(response);
            stream.WriteLine(response);
            stream.Flush();
        }
    }
}
