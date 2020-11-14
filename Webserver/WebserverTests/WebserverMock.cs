using NUnit.Framework;
using Moq;
using Webserver;
using System.IO;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace WebserverTests
{
    public class Tests
    {
        [Test]
        public void Test()
        {
            const string DemoRequest = "GET /messages/1 HTTP/1.1\r\nHost: localhost: 2200\r\n";
            var rs = new MemoryStream(Encoding.UTF8.GetBytes(DemoRequest));
            var ws = new MemoryStream();
            var client = new Mock<ITcpClient>();
            client.Setup(c => c.GetReadStream()).Returns(rs);
            client.Setup(c => c.GetWriteStream()).Returns(ws);
            var server = new Mock<HTTPServer>();
            server.Object.HandleClient(client.Object);
            Assert.AreEqual("HTTP/1.1 404 NotFound\nContent-Length: 0\n\r\n", Encoding.UTF8.GetString(ws.ToArray()));
        }
    }
}