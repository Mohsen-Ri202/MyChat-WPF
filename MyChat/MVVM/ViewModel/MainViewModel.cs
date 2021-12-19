using MyChat.MVVM.Core;
using MyChat.MVVM.Model;
using MyChat.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyChat.MVVM.ViewModel
{
    class MainViewModel
    {

        public ObservableCollection<UserModel> Users { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }       
        public RelayCommand SendMessageCommand { get; set; }       
        private Server _server;

        public string UserName { get; set; }
        public string Message { get; set; }

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(UserName), o=>!string.IsNullOrEmpty(UserName));
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(UserName), o=>!string.IsNullOrEmpty(Message));
        }



        private void UserConnected()
        {
            var user = new UserModel
            {
                UserName = _server.packetReader.ReadMessage(),
                UID = _server.packetReader.ReadMessage()
            };
            if (!Users.Any(o=>o.UID==user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
