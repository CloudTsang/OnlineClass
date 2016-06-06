using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOnlineMVC.tol.coder
{
    /// <summary> 编解码器 </summary>
    public class TolCoder:ICoder
    {
        private IEnCode _enCmd;
        private IDeCode _deCmd;
        /// <param name="en">编码方法</param>
        /// <param name="de">解码方法</param>
        public TolCoder(IEnCode en=null, IDeCode de=null)
        {
            var tmp = new DENCode_Empty();
            if (en != null) _enCmd = en;
            else _enCmd = tmp;
            if (de != null) _deCmd = de;
            else _deCmd = tmp;
        }
        public string Encode(string str)
        {
            return _enCmd.encode(str);
        }

        public string Decode(string str)
        {
            return _deCmd.decode(str);
        }
    }
}