using System.Diagnostics;
using System.Web.Mvc;
using TestOnlineMVC.tol.coder;

namespace TestOnlineMVC.tol.filter
{
    public class TestFilter : ActionFilterAttribute, IActionFilter
    {
        public TolCoder coder;
        public int co;
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var content = new ContentResult();
            content.Content = "fasfasfd";
            var dic = new TempDataDictionary();
            dic["haha"] = 3;
            filterContext.Controller.TempData = dic;
            
            Basic.trace(filterContext.ActionParameters["info"]);
            filterContext.ActionParameters["info"]="okay";
//            filterContext.Result = content; //之后的全部不运行
//            HttpContext hc = new HttpContext()
//            ControllerContext cc= new ControllerContext(filterContext.HttpContext,filterContext.RouteData,filterContext.Controller);
//            filterContext.ActionDescriptor.Execute(cc, new Dictionary<string, object> {{"info", "yoeueu"}});
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Basic.trace("co:"+co);
            ContentResult res = (ContentResult)filterContext.Result;
            Basic.trace(res.Content);
            Basic.trace("param:"+filterContext.ActionDescriptor.GetParameters());
            var con = (ContentResult) filterContext.Result;          
            Basic.trace(con.Content);       
//            filterContext.Result = con; //运行到最后
        }
    }
    public class TestFilter2 : FilterAttribute , IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var content = new ContentResult();
            content.Content = "1234566788";
            
            var dic = filterContext.Controller.TempData;
            dic["hahaha"] = 4;
            foreach (var i in dic)
            {
                Basic.trace(i.Key+"    "+i.Value);
            }
            filterContext.Controller.TempData = dic;
//                        filterContext.Result = content; //Action和f2的ed不运行
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var con = new ContentResult();
            con.Content = "wwwwwwwwwww";
            Basic.trace("filter2");
//            filterContext.Result = con;//运行到最后
        }
    }
    public class TestFilter3 : FilterAttribute,IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Basic.trace("filter3");
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}