using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Timers;
namespace TestOnlineMVC.tol.net.clientmgr
{
    public abstract class LoginClientMgr_Template : IClientMgr
    {
        protected List<Client> _list;
        protected Timer _timer;

        public LoginClientMgr_Template()
        {
            _list = new List<Client>();
        }

        #region 用户登陆操作相关
        public virtual void Login(String num, String sid)
        {
            Add<Client>(num, sid);
        }

        public virtual void Logout(String sid)
        {
            Basic.trace(sid + " 登出了。");
            _list.Remove(searchBySid<Client>(sid));
        }

        /// <summary> 增加用户，已经存在账号num的用户时视为异常登录将原用户设为 踢出 状态 </summary>
        protected virtual void Add<T>(String num, String sid) where T : Client
        {
            var tmp = searchByNumber<T>(num);
            Basic.trace((tmp==null).ToString());
            try
            {
                Basic.trace(tmp.Count.ToString()); 
            }
            catch (Exception err) { Basic.trace(err.StackTrace);}

            
            //用户已登陆，将旧登陆记录踢出登陆组          
            if (tmp != null)
            {
                foreach (var i in tmp)
                {
                    if (i.status == Client.STATUS_BAN) continue;
                    i.ban();
                    Basic.trace(i.sid + " 被踢出。");
                }
            }
            T newcli = (T)Activator.CreateInstance(typeof(T), num, sid);
            _list.Add(newcli);
            Basic.trace(sid + " 登陆了。");
        }
        #endregion

        /// <summary>删除用户</summary>
        public virtual void Delete(String sid)
        {
            _list.Remove(searchBySid<Client>(sid));
            Basic.trace(sid + " 被彻底删除");
        }
        
        /// <summary> 接收到心跳包时更新登录用户的检测时间 </summary>
        public virtual void heartBeat(String sid)
        {
            searchBySid<Client>(sid).time = DateTime.Now.AddMinutes(Basic.HB_SPAN);
            Basic.trace("更新 " + sid + " 检测时间");
        }

        #region 用户状态相关
        public virtual Boolean isLogin(String sid)
        {
            var tmp = searchBySid<Client>(sid);
            return tmp != null && tmp.status == Client.STATUS_ONLINE;
        }

        public virtual Boolean isBan(String sid)
        {
            var tmp = searchBySid<Client>(sid);
            return tmp != null && tmp.status == Client.STATUS_BAN;
        }
        #endregion

        #region 检索相关
        /// <summary> 搜索全部账号为num的登录用户 </summary>
        protected virtual List<T> searchByNumber<T>(String num) where T : Client
        {
            var tmp = _list.FindAll((cli) => { return cli.number == num; });
            var ret = tmp.ConvertAll<T>(x => (T)x).ToList();
            return ret;
        }

        protected virtual T searchBySid<T>(String sid) where T : Client
        {
            return (T)_list.Find((cli) => { return cli.sid == sid; });
        }
        #endregion

        public abstract void startHeartBeatTest();

        public virtual void stopHeartBeatTest()
        {
            _timer.Stop();
            _timer.Dispose();
        }

    }
}