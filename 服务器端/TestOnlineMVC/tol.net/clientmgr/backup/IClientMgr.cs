using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOnlineMVC.tol.net.clientmgr
{
    public interface IClientMgr
    {
        void Login(String num, String sid);
        void Logout(String sid);

        /// <summary>
        /// 删除被ban的用户
        /// </summary>
        void Delete(String sid);

        Boolean isLogin(String sid);
        Boolean isBan(String sid);

        /// <summary>
        /// 接收到心跳包时更新登录用户的检测时间
        /// </summary>
        void heartBeat(String sid);
        /// <summary>
        /// 每5个小时删除未作心跳检测时间更新的用户，
        /// 登陆用户每次发来（心跳）请求时更新时间，
        /// 被踢出用户在发来（心跳）请求时会告知异地登录后删除。
        /// </summary>
        void startHeartBeatTest();

        void stopHeartBeatTest();
    }
}
