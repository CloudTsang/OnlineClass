using System;
using System.Web;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.net
{
    public abstract class Cookies_Template : ICookies
    {
        public abstract void login(IInfo cli);

        public abstract String verify();

        public abstract void logout();

        public virtual string getCkProp(string prop , string name)
        {
            return getCookie(name)[prop];
        }

        /// <summary> 写入cookie </summary>
        protected void addCookie(HttpCookie ck)
        {
            HttpContext.Current.Response.Cookies.Add(ck);
        }

        ///<summary>获取http请求的cookie</summary>
        protected HttpCookie getCookie(String name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }
    }
}