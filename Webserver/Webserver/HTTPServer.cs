using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Linq;

namespace Webserver
{
    public class HTTPServer
    {
        private TcpListener server;
        private Dictionary<(string verb, string path), Action<RequestContext, StreamWriter>> routes = new Dictionary<(string verb, string path), Action<RequestContext, StreamWriter>>();
        public HTTPServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            Task t = new Task(Start);
            t.Start();
        }
        //Ok,Created,NotFound,BadRequest,InternalServerError,methodNotAllowed(get auf post route), LengthRequired
        public void Start()
        {
            try
            {
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Task.Run(() => HandleClient(client));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                
            }
            
        }
        public void RegisterRoute(string verb, string path, Action<RequestContext, StreamWriter> action) => routes.Add((verb, path), action);
        private void HandleClient(TcpClient client)
        {
            using StreamReader sr = new StreamReader(client.GetStream());
            using StreamWriter sw = new StreamWriter(client.GetStream());
            Dictionary<string, string> header = new Dictionary<string, string>();
            Console.WriteLine("Client connected");

            string message = sr.ReadLine(); //Erste Line von dem HTTP Request
            string[] splits = message.Split(" ");          
            string verb = splits[0]; 
            string path = splits[1];
            string httpVersion = splits[2];

            Console.WriteLine("Headers: ");
            while (!sr.EndOfStream)
            {
                message = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(message))
                    break;
                Console.WriteLine(message);
                int indexof = message.IndexOf(':');

                if (indexof < 0)
                    continue;

                header.Add(message.Substring(0, indexof), message.Substring(indexof + 1).Trim(' '));
            }

            var contentset=header.Where((val) => val.Key == "Content-Length").FirstOrDefault();
            int contentlength=0;
            int.TryParse(contentset.Value, out contentlength);
            char[] payload = new char[contentlength];
            int requestedID = 0;
            string h = path.Substring(path.LastIndexOf('/')+1);
            int.TryParse(h, out requestedID);
            sr.ReadBlock(payload);
            RequestContext rc = new RequestContext(verb, path, httpVersion, header, new string(payload), requestedID);
            var res = routes.Where((keyval, action) =>
            {
                int indexofSlash = path.LastIndexOf('/');
                Console.WriteLine(path.Substring(0, indexofSlash));
                return keyval.Key.verb == verb && keyval.Key.path.Trim('*') == (indexofSlash == 0 ? path : path.Substring(0, indexofSlash + 1));
            }).FirstOrDefault();
            if (res.Value == null)
            {
                Console.WriteLine("NotFound");
                SendError(sw,HttpStatusCode.NotFound);
                return;
            }
            res.Value(rc, sw);
            //SendSuccess(sw, "");
        }
        public static void SendSuccess(StreamWriter stream, HttpStatusCode statuscode, string message)
        {
            string response = $"HTTP/1.1 {(int)statuscode} {statuscode}\nContent-Length: {message.Length}\nContent-Type: text/plain; charset=utf-8\n\n{message}";
            //Console.WriteLine(response);
            stream.WriteLine(response);
            stream.Flush();
        }
        public static void SendError(StreamWriter stream, HttpStatusCode statuscode)
        {
            string message = "Ressource Not Found";
            string response = $"HTTP/1.1 {(int)statuscode} {statuscode}\nContent-Length: {message.Length}\nContent-Type: text/plain; charset=utf-8\n\n{message}";
            //Console.WriteLine(response);
            stream.WriteLine(response);
            stream.Flush();
        }
    }
}
