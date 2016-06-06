using System;
using TestOnlineMVC.tol.net;

namespace TestOnlineMVC.Controllers
{
    public class HeartBeatController : PostController_Template
    {
        public String student()
        {
            Basic.trace("来自 " + System.Web.HttpContext.Current.Session.SessionID + " 的心跳请求");
            return StudentCookies.instance.verify();            
        }
    }
}