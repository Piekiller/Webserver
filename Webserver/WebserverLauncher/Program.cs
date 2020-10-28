using System;
using System.Threading;
using Webserver;
namespace WebserverLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            HTTPServer hTTP = new HTTPServer(10000);
            hTTP.Start();
            Thread.Sleep(5000);           
            Console.ReadKey();
        }
    }
}
