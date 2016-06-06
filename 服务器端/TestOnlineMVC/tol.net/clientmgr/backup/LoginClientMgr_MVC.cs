using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace TestOnlineMVC.tol.net.clientmgr
{
    public class LoginClientMgr_MVC : LoginClientMgr_Template
    {
        #region 单例类
        private static LoginClientMgr_MVC _ins;
        public static void init() { if (_ins == null)_ins = new LoginClientMgr_MVC(); }
        public static LoginClientMgr_MVC instance
        {
            get { return _ins; }
        }
        #endregion

        /// <summary>
        /// 每5个小时删除未作心跳检测时间更新的用户，
        /// 登陆用户每次发来（心跳）请求时更新时间，
        /// 被踢出用户在发来（心跳）请求时会告知异地登录后删除。
        /// </summary>
        public override void startHeartBeatTest()
        {
            _timer = new Timer(5 * 60 * 60 * 1000);
            _timer.Elapsed += new ElapsedEventHandler(
               (object sender, ElapsedEventArgs args) =>
               {
                   Task t1 = new Task(() =>
                   {
                       foreach (var i in _ban)
                       {
                           TimeSpan t = DateTime.Now - i.time;
                           if (t > TimeSpan.FromDays(1)) _ban.Remove(i);
                       }
                   });
                   Task t2 = new Task(() =>
                   {
                       foreach (var j in _list)
                       {
                           TimeSpan t = DateTime.Now - j.time;
                           if (t > TimeSpan.FromDays(1)) _list.Remove(j);
                       }
                   });
                   Task.WaitAll(t1, t2);
               }
               );
            _timer.AutoReset = false;
            _timer.Start();
        }
    }
}