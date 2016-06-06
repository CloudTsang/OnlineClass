using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestOnlineMVC.tol.net;

namespace TestOnlineMVC.tol.filter
{
    public class AuthenFilter:FilterAttribute,IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var auth = StudentCookies.instance.verify();
            if(auth!="success") filterContext.setResultContent(auth);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}