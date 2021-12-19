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
        public ObservableCollection<string> Messages { get; set; }

        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        private Server _server;

        public string UserName { get; set; }
        public string Message { get; set; }

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.connectedEvent += MessageReceived;
            _server.connectedEvent += RemoveUser;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(UserName), o=>!string.IsNullOrEmpty(UserName));
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o=>!string.IsNullOrEmpty(Message));
        }

        private void MessageReceived()
        {
            var msg = _server.packetReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
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
        private void RemoveUser()
        {
            var uid = _server.packetReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }
    }
}
