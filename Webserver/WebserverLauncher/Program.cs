using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using Webserver;
namespace WebserverLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            HTTPServer http = new HTTPServer(10000);
            Messages m = new Messages();
            http.RegisterRoute("GET", "/messages", (RequestContext rc, StreamWriter sw)=>
            {
                HTTPServer.SendSuccess(sw, HttpStatusCode.OK,m.GetMessages());
            });
            http.RegisterRoute("POST", "/messages", (RequestContext rc, StreamWriter sw) =>
            {
                string payload = rc.Payload;
                HTTPServer.SendSuccess(sw, HttpStatusCode.Created, m.PostMessage(payload).ToString());
            });
            http.RegisterRoute("GET", "/messages/*", (RequestContext rc, StreamWriter sw) =>
            {
                string result = m.GetMessage(rc.Requestedfield);
                if(result==null)
                {
                    HTTPServer.SendError(sw,HttpStatusCode.NotFound);
                    return;
                }
                HTTPServer.SendSuccess(sw, HttpStatusCode.OK,result);
            });
            http.RegisterRoute("PUT", "/messages/*", (RequestContext rc, StreamWriter sw) =>
            {
                if (!m.PutMessage(rc.Requestedfield, rc.Payload))
                    HTTPServer.SendError(sw,HttpStatusCode.NotFound);
                HTTPServer.SendSuccess(sw, HttpStatusCode.Created,"" );
            });
            http.RegisterRoute("DELETE", "/messages/*", (RequestContext rc, StreamWriter sw) =>
            {
                if(!m.DeleteMessage(rc.Requestedfield))
                    HTTPServer.SendError(sw,HttpStatusCode.NotFound);
                HTTPServer.SendSuccess(sw, HttpStatusCode.OK, "");
            });
            http.Start();
            Thread.Sleep(5000);           
            Console.ReadKey();
        }
    }
}
