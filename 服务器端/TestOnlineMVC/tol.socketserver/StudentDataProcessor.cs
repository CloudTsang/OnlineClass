using System;
using TestOnlineMVC.tol.info;
using TestOnlineMVC.tol.net;
using TestOnlineMVC.tol.net.clientmgr;

namespace TestOnlineMVC.tol.socketserver
{
    public class StudentDataProcessor : IDataProcessor
    {
        protected IStudentEvt _evtHandler = new StudentEvtHandler_Socket();
        public String dataProcess(ISocketMsg msg)
        {
            if (msg.msgType != (int)Basic.dataType.登录口令) //传来登陆信息时无需验证
            {
                String dt = verify(msg.specifID);
                Basic.trace(msg.specifID+" 连接状态： "+dt);
                //客户端收取心跳检测返回信息的部分没有写成解码Base64，如果传来的是心跳包，则返回不编码为Base64
                if (msg.msgType == (int)Basic.dataType.心跳检测信息) return dt; 
                if (dt != Basic.StatusMessage.MSG_VERIFY_SUCCESS) return Basic.convertToBase64(dt);
            }
            switch (msg.msgType)
            {
                    //登陆信息
                case (int)Basic.dataType.登录口令:
                    var info = _evtHandler.loginHandler(msg.msgBody) as ClientInfo;
                    LoginClientMgr_Socket.instance.Login(info.number,info.sid);
                    break;
                    //试卷请求
                case (int)Basic.dataType.试题列表:
                    var tmp = _evtHandler.getQuestion(msg.msgBody);
                    LoginClientMgr_Socket.instance.testStart(msg.specifID , tmp.Key , tmp.Value);
                    break;
                    //批改答案
                case (int)Basic.dataType.答案:
                    var test = LoginClientMgr_Socket.instance.testStatus(msg.specifID);
                    if (test.Key == String.Empty) return Basic.convertToBase64("考试信息丢失!");
                    if (test.Value) _evtHandler.Score0(test.Key, msg.client);
                    else _evtHandler.getScore(msg.msgBody, test.Key, msg.client);
                    break;
                    //放弃考试
                case (int)Basic.dataType.放弃作答:
                    _evtHandler.giveup();
                    LoginClientMgr_Socket.instance.testOver(msg.specifID);
                    break;
                    //登出
                case (int)Basic.dataType.登出:
                    _evtHandler.logout();
                    LoginClientMgr_Socket.instance.Logout(msg.specifID);
                    break;
            }
            return _evtHandler.sendBack;
        }

        protected String verify(String sid)
        {
            if (LoginClientMgr_Socket.instance.isLogin(sid))
            {
                LoginClientMgr_Socket.instance.heartBeat(sid);
                return Basic.StatusMessage.MSG_VERIFY_SUCCESS;
            }
            if (LoginClientMgr_Socket.instance.isBan(sid)) return Basic.StatusMessage.MSG_ANOTHER_LOGIN;
            if (LoginClientMgr_Socket.instance.isOverTime(sid)) return Basic.StatusMessage.MSG_HBT_OVERTIME;
            return Basic.StatusMessage.MSG_NOT_LOGIN;
        }
    }
}