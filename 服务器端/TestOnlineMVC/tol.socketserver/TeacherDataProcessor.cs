using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using TestOnlineMVC.tol.net;

namespace TestOnlineMVC.tol.socketserver
{
    public class TeacherDataProcessor : IDataProcessor
    {
        protected TeacherEvtHandler _evtHandler = new TeacherEvtHandler();
        public String dataProcess(ISocketMsg msg)
        {
            switch (msg.msgType)
            {
                case (int)Basic.dataType.教师端登录:
                    _evtHandler.loginHandler();
                    break;
                case (int)Basic.dataType.试卷班级列表:
                    _evtHandler.classsHandler(msg.msgBody);
                    break;
                case (int)Basic.dataType.教师端分数信息:
                    _evtHandler.scoreHandler(msg.msgBody);
                    break;
                case (int)Basic.dataType.统计信息:
                    _evtHandler.statHandler(msg.msgBody);
                    break;
            }
            return _evtHandler.sendBack;
        }
    }
}