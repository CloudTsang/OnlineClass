using System;
using System.Web.Mvc;

namespace TestOnlineMVC.tol.filter
{
    public static class FilterMethod
    {
        /// <summary>获取返回结果的字符串内容</summary>
        public static String getResultContent(this ActionExecutingContext fc)
        {
            var c = (ContentResult)fc.Result;
            return c.Content;
        }
        /// <summary>获取返回结果的字符串内容</summary>
        public static String getResultContent(this ActionExecutedContext fc)
        {
            var c = (ContentResult)fc.Result;
            return c.Content;
        }

        /// <summary> 设置返回结果的字符串内容 </summary>
        public static void setResultContent(this ActionExecutingContext fc, string str)
        {
            var c = new ContentResult();
            c.Content = str;
            fc.Result = c;
        }
        /// <summary> 设置返回结果的字符串内容 </summary>
        public static void setResultContent(this ActionExecutedContext fc, string str)
        {
            var c = new ContentResult();
            c.Content = str;
            fc.Result = c;
        }
    }
}