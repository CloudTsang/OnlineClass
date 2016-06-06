using System.Threading.Tasks;
using System.Timers;

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
        /// 每1个小时删除未作心跳检测时间更新的用户，
        /// 登陆用户每次发来（心跳）请求时更新时间，
        /// 被踢出用户在发来（心跳）请求时会告知异地登录后删除。
        /// </summary>
        public override void startHeartBeatTest()
        {
            _timer = new Timer(1 * 60 * 60 * 1000);
            _timer.Elapsed += new ElapsedEventHandler(
               (object sender, ElapsedEventArgs args) =>
               {
                   Task t = new Task(() =>
                   {
                       foreach (var cli in _list)
                       {
                           if (!cli.isOnline()) Delete(cli.sid);
                       }
                   });
                   t.Start();
               }
               );
            _timer.AutoReset = true;
            _timer.Start();
        }
    }
}