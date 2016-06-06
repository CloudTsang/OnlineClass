using System;
using System.Web.Mvc;
using TestOnlineMVC.tol.filter;
using TestOnlineMVC.tol.info;
using TestOnlineMVC.tol.net;
namespace TestOnlineMVC.Controllers
{
    /// <summary> 学生端请求控制器 模板 </summary>
    public class TemplateStudentController : Controller
    {
        protected IStudentEvt _hdl;
        protected StudentCookies _coo;

        public TemplateStudentController ()
        {
            _hdl = new StudentEvtHandler();
            _coo = StudentCookies.instance;
        }

        public virtual String login(String info = null)
        {
            var student = _hdl.loginHandler(info) as StudentInfo;
            if (student != null) _coo.login(student);
            return _hdl.sendBack;
        }
        [AuthenFilter(Order = 2)]
        public virtual String testRequest(String info = null)
        {
//            var msg = _coo.verify();
//            if (msg != Basic.StatusMessage.MSG_VERIFY_SUCCESS) return Basic.convertToBase64(msg);

            var res = _hdl.getQuestion(info);
            if (res.Key != String.Empty) _coo.testChosen(res.Key, res.Value);
            return _hdl.sendBack;
        }

        [AuthenFilter(Order = 2)]
        public virtual String answerCheck(String info = null)
        {
//            var msg = _coo.verify();
//            if (msg != Basic.StatusMessage.MSG_VERIFY_SUCCESS) return Basic.convertToBase64(msg);

            if (_coo.isOverTime())
            {
                _hdl.Score0(_coo.testName, _coo.number);
                Basic.trace(_coo.number + "考试超时记为0分");
            }
            else
            {
                _hdl.getScore(info, _coo.testName, _coo.number);
            }
            _coo.testOver();
            return _hdl.sendBack;
        }

        [AuthenFilter(Order = 2)]
        public virtual String giveup(String info = null)
        {
//            var msg = _coo.verify();
//            if (msg != Basic.StatusMessage.MSG_VERIFY_SUCCESS) return Basic.convertToBase64(msg);
            _coo.testOver();
            _hdl.giveup();
            return _hdl.sendBack;
        }

        public virtual String logout()
        {
            _coo.logout();
            _hdl.logout();
            return _hdl.sendBack;
        }
    }
}