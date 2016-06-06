using System;
using System.Web.Mvc;
using TestOnlineMVC.tol.net;

namespace TestOnlineMVC.Controllers
{
    public class TemplateTeacherController : Controller
    {
        protected TeacherEvtHandler _hdl = new TeacherEvtHandler();
        public virtual String login()
        {
            _hdl.loginHandler();
            return _hdl.sendBack;
        }
        ///<summary>发回班级列表，info应当是1个试卷名的字符串</summary>
        public virtual String classListRequest(String info)
        {
            _hdl.classsHandler(info);
            return _hdl.sendBack;
        }
        ///<summary>发回分数，info应当是1个数组的json字符串，有2个元素[0]试卷名，[1]班级名</summary>
        public virtual String scoreRequest(String info)
        {
            _hdl.scoreHandler(info);
            return _hdl.sendBack;
        }
        ///<summary>发回统计结果，info应当是1个试卷名的字符串</summary>
        public virtual String statRequest(String info)
        {
            _hdl.statHandler(info);
            return _hdl.sendBack;
        }
    }
}