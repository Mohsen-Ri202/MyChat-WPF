﻿using MyChat.Net.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static TcpListener _listener;
        static List<Client> _users;
        static void Main(string[] args)
        {
            _users = new List<Client>();
           
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            
            _listener.Start();
            var client = new Client(_listener.AcceptTcpClient());
            _users.Add(client);

            BroadcastConnection();
        }
        static void BroadcastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.UserName);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _users.Find(x => x.UID.ToString() == uid);
            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }
            BroadcastMessage($"{disconnectedUser} has been disconnected");
        }
    }
}
