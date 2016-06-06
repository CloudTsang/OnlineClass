using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace TestOnlineMVC
{
    public static class GlobMethod
    {
        public static String convertToJson(this Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}