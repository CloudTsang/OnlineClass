using System;
using System.Collections.Generic;
using System.Timers;
namespace TestOnlineMVC.tol.net.clientmgr
{
    public abstract class LoginClientMgr_Template : IClientMgr
    {
        protected List<Client> _list = new List<Client>();
        protected List<Client> _ban = new List<Client>();
        protected Timer _timer;

        public virtual void Login(String num, String sid)
        {
            var tmp = searchByNumber(num, _list);
            //用户已登陆，将旧登陆记录踢出登陆组
            if (tmp != null)
            {
                _ban.Add(tmp);
                _list.Remove(tmp);
                Basic.trace(tmp.sid + " 被踢出。");
            }
            _list.Add(new Client(num, sid));
            Basic.trace(sid + " 登陆了。");
        }
        public virtual void Logout(String sid)
        {
            Basic.trace(sid + " 登出了。");
            _list.Remove(searchBySid(sid, _list));
        }
        /// <summary>
        /// 接收到心跳包时更新登录用户的检测时间
        /// </summary>
        public virtual void heartBeat(String sid)
        {
            searchBySid(sid, _list).time = DateTime.Now;
            Basic.trace("更新 " + sid + " 检测时间");
        }
        /// <summary>
        /// 删除被ban的用户
        /// </summary>
        public virtual void Delete(String sid)
        {
            _ban.Remove(searchBySid(sid, _ban));
            Basic.trace(sid + " 被彻底删除");
        }

        public virtual Boolean isLogin(String sid)
        {
            return _list.Exists((cli) => { return cli.sid == sid; });
        }

        public virtual Boolean isBan(String sid)
        {
            return _ban.Exists((cli) => { return cli.sid == sid; });
        }

        protected virtual T searchByNumber<T>(String num, List<T> list) where  T:Client
        {
            return list.Find((cli) => { return cli.number == num; });
        }

        protected virtual T searchBySid<T>(String sid, List<T> list) where T:Client
        {
            return list.Find((cli) => { return cli.sid == sid; });
        }

        public abstract void startHeartBeatTest();      

        public virtual void stopHeartBeatTest()
        {
            _timer.Stop();
            _timer.Dispose();
        }
     
    }
}