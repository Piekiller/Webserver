using Moq;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading;
using Webserver;
namespace Webserver.Test
{
    public class Tests
    {
        [Test]
        public void Test()
        {
            const string incomingRequest = "GET /messages/1 HTTP/1.1\r\nHost: localhost: 2200\r\n";
            var rs = new MemoryStream(Encoding.UTF8.GetBytes(incomingRequest));
            var ws = new MemoryStream();
            var client = new Mock<Webserver.ITcpClient>();
            client.Setup(c => c.GetReadStream()).Returns(rs);
            client.Setup(c => c.GetWriteStream()).Returns(ws);
            var listener = new Mock<ITcpListener>();
            var web = new WebServer(listener.Object);
            web.HandleClient();

            Thread.Sleep(500); // ToDo: make this nicer
            Assert.AreEqual("HTTP/1.1 404 NotFound\r\n", Encoding.UTF8.GetString(ws.ToArray()));
        }
    }
}