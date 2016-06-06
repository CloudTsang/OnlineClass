using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TestOnlineMVC.tol.coder
{
    /// <summary> Base64编码 </summary>
    public class Encode_Base64:IEnCode
    {
        public string encode(string str)
        {
            byte[] barr = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(barr);
        }
    }
}