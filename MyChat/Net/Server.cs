using ChatServer.Net.IO;
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
        public PacketReader packetReader;

        public event Action connectedEvent;
        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);

                packetReader = new PacketReader(_client.GetStream());

                var connectionPacket = new PacketBuilder();
                connectionPacket.WriteOpCode(0);
                connectionPacket.WriteMessage(username);
                _client.Client.Send(connectionPacket.GetPacketBytes());
            }
            ReadPackets();
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("ah yes...");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(0);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
