using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOnlineMVC.tol.coder
{
    public class TolCoderSt
    {
        public enum CoderNum
        {
           无 = -1,
           Base64 = 1
        }

        public static IEnCode getEncoder(CoderNum num)
        {
            
            switch (num)
            {
                case CoderNum.Base64:
                    return new Encode_Base64();
                default:
                    return new DENCode_Empty();
            }
        }

        public static IDeCode getDecoder(CoderNum num)
        {
            switch (num)
            {
                case CoderNum.Base64:
                    return new Decode_Base64();
                default:
                    return new DENCode_Empty();
            }
        }
    }
}