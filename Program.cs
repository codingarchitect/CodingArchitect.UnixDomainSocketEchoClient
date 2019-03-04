using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CodingArchitect.UnixDomainSocketEchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var socketPath = GetSocketPath();
            Console.WriteLine("Hello World!");
            SetupClient(socketPath);
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        public static async void SetupClient(string path)
        {
          SocketClient client = new SocketClient(new UnixDomainSocketEndPoint(path));
          var request = "Hi from programmatic client";
          var response = await client.SendRequest(request);
          Console.WriteLine("[Client] Server responded with '{0}' for the programmatic client's Request '{1}'", 
              request, response);
        }

        private static string GetSocketPath()
        {
            var result = Path.Combine(Path.GetTempPath() + "echoserver.sock");
            return result;
        }
    }
}
