using MyChat.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Net
{
    class Server
    {
        TcpClient _client;
        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                var connectionPacket = new PacketBuilder();
                connectionPacket.WriteOpCode(0);
                connectionPacket.WriteString(username);
                _client.Client.Send(connectionPacket.GetPacketBytes());
            }
        }
    }
}
