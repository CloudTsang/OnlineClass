using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using TestOnlineMVC.tol.filter;
using TestOnlineMVC.tol.net;
using TestOnlineMVC.tol.net.clientmgr;
using TestOnlineMVC.tol.socketserver;

namespace TestOnlineMVC.Controllers
{
    public class HomeController : Controller
    {
        public String getBase64(String info)
        {
            return Basic.convertToBase64(info);
        }

        public String Index()
        {
//            SocketServer.bootSocketServer("nicepass");
            LoginClientMgr_MVC.init();
            LoginClientMgr_MVC.instance.startHeartBeatTest();
            return "欢迎使用安卓网络课程APP";
        }

        public void openSocket(String password="*")
        {
                SocketServer.bootSocketServer(password);
        }

        public void closeSocket(String password = "*")
        {
            SocketServer.shutdownSocketServer(password);
        }
    }
}