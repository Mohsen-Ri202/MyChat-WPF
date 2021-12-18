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
        }
    }
}
