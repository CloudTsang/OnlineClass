using System.Web.Mvc;
using TestOnlineMVC.tol.coder;
using TestOnlineMVC.tol.net;

namespace TestOnlineMVC.tol.filter
{
    /// <summary>
    /// post请求的编解码filter ， 解码默认为无， 编码默认为base64
    /// </summary>
    public class CryptFilter_POST:CryptFilter_Template
    {

        protected override string getMessage(ActionExecutingContext filterContext)
        {
            return  PostMsgGetter.GetPostStr();
        }

        public CryptFilter_POST(TolCoderSt.CoderNum e = TolCoderSt.CoderNum.Base64, TolCoderSt.CoderNum d = TolCoderSt.CoderNum.无) : base(e, d)
        {
        }
    }
}