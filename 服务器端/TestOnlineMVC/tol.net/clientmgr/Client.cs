using System;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.net.clientmgr
{
    /// <summary>基础登录用户类</summary>
    public class Client : ClientInfo , IClient
    {
        protected DateTime _hb;
        protected int _status;
        /// <summary>正常在线状态</summary>
        public const int STATUS_ONLINE = 0;
        /// <summary>登录异常被踢出状态</summary>
        public const int STATUS_BAN = 1;
        /// <summary>超时未心跳检测状态（视作已登出）</summary>
        public const int STATUS_OVERTIME = 2;
        public Client(String n, String id):base(n,id)
        {
            _num = n;
            _sid = id;
            _hb = DateTime.Now.AddMinutes(Basic.HB_SPAN);
            _status = STATUS_ONLINE;
        }
        
        ///<summary>心跳检测过期时间</summary>
        public DateTime time
        {
            get { return _hb;}
            set { _hb = value; }
        }
        /// <summary>用户在线状态</summary>
        public int status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>将用户踢出，并且将检测间隔延长至n小时，这期间该用户发来请求时将返回 登录异常 信息</summary>
        public virtual void ban()
        {
            _status = STATUS_BAN;
            _hb = DateTime.Now.AddHours(1);
        }
        /// <summary> 设置超时未心跳检测状态</summary>
        public virtual void overTime()
        {
            _status = STATUS_OVERTIME;
            _hb = DateTime.Now.AddHours(0.5);
        }
        ///<summary>与心跳检测的过期时间比较，连续进行n次都超时未传来心跳包的用户视作已下线返回false</summary>
        public virtual Boolean isOnline()
        {
            TimeSpan span =  _hb - DateTime.Now;
            if (span.Seconds < 0)
            {
                return false;
            }
            return true;
        }
    }
}