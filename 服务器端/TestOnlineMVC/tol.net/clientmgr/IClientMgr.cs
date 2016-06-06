using System;
namespace TestOnlineMVC.tol.net.clientmgr
{
    /// <summary> 登录用户管理器基础功能接口 </summary>
    public interface IClientMgr
    {
        void Login(String num, String sid);
        void Logout(String sid);
        /// <summary> 删除被ban的用户 </summary>
        void Delete(String sid);
        Boolean isLogin(String sid);
        Boolean isBan(String sid);
        /// <summary> 接收到心跳包时更新登录用户的检测时间 </summary>
        void heartBeat(String sid);
        void startHeartBeatTest();
        void stopHeartBeatTest();
    }
}
