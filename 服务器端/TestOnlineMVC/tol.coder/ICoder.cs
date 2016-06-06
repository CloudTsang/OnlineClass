using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOnlineMVC.tol.coder
{
    public interface ICoder
    {
        String Encode(String str);
        String Decode(String str);
    }
}
