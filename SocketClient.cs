using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CodingArchitect.UnixDomainSocketEchoClient
{
  public class SocketClient
  {
    private readonly EndPoint address;
    private Socket client;
    public SocketClient(EndPoint address)
    {
      this.address = address;
    }
    
    public async Task<string> SendRequest(string request)
    {
      try {
        
        var protocolType = address.AddressFamily == AddressFamily.InterNetwork ?
        ProtocolType.Tcp : ProtocolType.Unspecified;
        client = new Socket(address.AddressFamily, SocketType.Stream, protocolType);
        await client.ConnectAsync(address);

        using(NetworkStream networkStream = new NetworkStream(client))
        using(StreamWriter writer = new StreamWriter(networkStream))
        {
          writer.AutoFlush = true;
          await writer.WriteAsync(request);
          var buffer = new byte[4096];
          var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
          var response = Encoding.UTF8.GetString(buffer, 0, byteCount);
          client.Close();
          return response;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
}