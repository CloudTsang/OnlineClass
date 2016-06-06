using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOnlineMVC.tol.coder
{
    /// <summary> 什么都不做的编解码指令 </summary>
    public class DENCode_Empty:IDeCode,IEnCode
    {
        public string decode(string str)
        {
            return str;
        }

        public string encode(string str)
        {
            return str;
        }
    }
}