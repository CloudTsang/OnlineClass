using System;
using System.Web.Mvc;
using TestOnlineMVC.tol.coder;

namespace TestOnlineMVC.tol.filter
{
    /// <summary>
    /// Get请求的编解码filter ， 默认使用Base64
    /// </summary>
    public class CryptFilter_GET:CryptFilter_Template
    {
        public CryptFilter_GET(TolCoderSt.CoderNum e = TolCoderSt.CoderNum.Base64,
            TolCoderSt.CoderNum d = TolCoderSt.CoderNum.Base64) : base(e, d)
        {
        }

        protected override string getMessage(ActionExecutingContext filterContext)
        {
            return (String)filterContext.ActionParameters["info"];
        }
    }
}