using System;
using System.Threading.Tasks;
using TestOnlineMVC.tol.net.clientmgr;
using TestOnlineMVC.tol.socketserver.connection;

namespace TestOnlineMVC.tol.socketserver
{
    public class SocketServer
    {
        /// <summary> 是否自动开启socket服务器，
        /// 这个值为false的场合，需要访问主页的openSocket控制器并输入密码作参数才会开启 </summary>
        private static Boolean isBoot = true;
        public static SocketListener soServer;
        protected static Task task;

        public static void bootSocketServer(String password)
        {
            if (!isBoot && password!="nicepass") return;
            if (soServer != null) return;

            LoginClientMgr_Socket.init();
            soServer = new SocketListener();
            task = new Task(() =>
            {
                soServer.startListening(Basic.getIP(), 9985);
            });
            task.Start();
            LoginClientMgr_Socket.instance.startHeartBeatTest();
            Basic.trace("Socket服务器已打开");
        }

        public static void shutdownSocketServer(String password)
        {
            if (!isBoot && password != "nicepass") return;
            if (soServer == null) return;
            soServer.stopListening();
            Basic.trace("Socket服务器已关闭");
        }
    }
}