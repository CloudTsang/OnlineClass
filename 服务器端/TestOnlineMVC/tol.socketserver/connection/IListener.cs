using System;
using System.Net.Sockets;

namespace TestOnlineMVC.tol.socketserver.connection
{
    /// <summary> socket侦听器基础功能接口 </summary>
    interface IListener
    {
        void startListening(String host, int port, int backlog = 100);
        void Send(Socket cli ,String message);
        void stopListening();
    }
}
