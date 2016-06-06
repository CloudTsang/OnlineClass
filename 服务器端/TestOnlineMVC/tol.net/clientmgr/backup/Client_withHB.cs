using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOnlineMVC.tol.net.clientmgr
{
    /// <summary>
    /// 有超时检测的登陆客户，适用于Socket
    /// </summary>
    public class Client_withHB : Client
    {
        protected int _status;
        public const int STATUS_ONLINE = 0;
        public const int STATUS_OVERTIME = 1;
        public const int STATUS_BAN = 2;
        public Client_withHB(string n, string id) : base(n, id)
        {
            _hb.AddMinutes(Basic.HB_SPAN);
            _status = STATUS_ONLINE;
        }

        public int status
        {
            get { return _status; }
            set { _status = value; }
        }

        ///<summary>与心跳检测的过期时间比较，连续进行n次都超时未传来心跳包的用户视作已下线返回false</summary>
        public override Boolean isOnline()
        {
            TimeSpan span = DateTime.Now - _hb;
            if (span.Minutes < 0)
            {
                _status = STATUS_OVERTIME;
                return false;
            }
            return true;
        }
    }
}