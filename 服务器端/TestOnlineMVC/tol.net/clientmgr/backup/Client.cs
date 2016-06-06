using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOnlineMVC.tol.net.clientmgr
{
    /// <summary>
    /// 基础登录用户类
    /// </summary>
    public class Client
    {
        protected String _num;
        protected String _sid;
        protected DateTime _hb;
        
        public Client(String n, String id)
        {
            _num = n;
            _sid = id;
            _hb = DateTime.Now;
            
        }
        ///<summary>用户账号</summary>
        public string number
        {
            get { return _num; }
        }

        ///<summary>特征码，使用mvc时为SessionID，使用socket时为？</summary>
        public String sid
        {
            get { return _sid; }
        }
        ///<summary>心跳检测过期时间</summary>
        public DateTime time
        {
            get { return _hb;}
            set { _hb = value; }
        }

        public virtual Boolean isOnline()
        {
            return true;
        }
    }
}