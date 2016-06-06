using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TestOnlineMVC.tol.coder
{
    /// <summary> Base64解码 </summary>
    public class Decode_Base64:IDeCode
    {
        public string decode(string str)
        {
            byte[] barr = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(barr);
        }
    }
}