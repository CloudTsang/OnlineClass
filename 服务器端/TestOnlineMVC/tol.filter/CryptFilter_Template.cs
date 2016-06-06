using System;
using System.Web.Mvc;
using TestOnlineMVC.tol.coder;

namespace TestOnlineMVC.tol.filter
{
    /// <summary>
    /// 信息编解码filter-模板
    /// 设置DeCoderNum、EncoderNum来指定解码方式和编码方式
    /// </summary>
    public abstract class CryptFilter_Template : FilterAttribute, IActionFilter
    {
        protected IEnCode en;
        protected IDeCode de;

        public CryptFilter_Template(TolCoderSt.CoderNum e , TolCoderSt.CoderNum d)
        {
            if (e == 0 || d == 0) throw new Exception("A De/Encoder should be set!");
                en = TolCoderSt.getEncoder(e);
                de = TolCoderSt.getDecoder(d);
        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            String str = getMessage(filterContext);
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    str = de.decode(str);
                    Basic.trace(str);
                }
                catch (Exception err)
                {
                    Basic.trace(err.StackTrace);
                    var res = new ContentResult();
                    res.Content = en.encode("传输信息错误！");
                    filterContext.Result = res;
                }
            }
            filterContext.ActionParameters["info"] = str;
        }

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
            String res = filterContext.getResultContent();
//                        Basic.trace(EncoderNum + " 编码 ： "+res);
            res = en.encode(res);
//            Basic.trace("得到返回结果 ： "+res);
            filterContext.setResultContent(res);
        }

        protected abstract String getMessage(ActionExecutingContext filterContext);
    }
}