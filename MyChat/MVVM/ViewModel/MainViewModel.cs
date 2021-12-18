using MyChat.MVVM.Core;
using MyChat.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.MVVM.ViewModel
{
    class MainViewModel
    {
        public RelayCommand ConnectToServer { get; set; }
        private Server _server;
        public string UserName { get; set; }
        public MainViewModel()
        {
            _server = new Server();
            ConnectToServer = new RelayCommand(o => _server.ConnectToServer(UserName), o=>!string.IsNullOrEmpty(UserName));
        }
    }
}
