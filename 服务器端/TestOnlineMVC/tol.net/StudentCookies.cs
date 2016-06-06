using System;
using System.Web;
using TestOnlineMVC.tol.info;
using TestOnlineMVC.tol.net.clientmgr;

namespace TestOnlineMVC.tol.net
{
    /// <summary>操作学生端cookie</summary>
    public class StudentCookies : Cookies_Template, IStudentCk
    {
        private static StudentCookies _inst;

        public static StudentCookies instance
        {
            get
            {
                if (_inst == null) _inst = new StudentCookies();
                return _inst;
            }
        }

        public override void login(IInfo cli)
        {
            #region 用户基本信息cookie
            HttpCookie ck = getCookie("student");
            if(ck==null) ck = new HttpCookie("student");
            ck["number"] = (cli as StudentInfo).number;
            ck["sid"] = HttpContext.Current.Session.SessionID;
            addCookie(ck);
            LoginClientMgr_MVC.instance.Login(ck["number"],ck["sid"]);
            #endregion

            #region 设置心跳检测限时
            HttpCookie hbck = new HttpCookie("heartbeat");
            hbck.Expires = DateTime.Now.AddMinutes(Basic.HB_SPAN);
            addCookie(hbck);
            #endregion
        }

        public override String verify()
        {
            HttpCookie ck = getCookie("student");
            //没有student cookie的情况：此用户从未登录
            if (ck == null)
            {
                Basic.trace("此用户未登录");
                return Basic.StatusMessage.MSG_NOT_LOGIN;
            }

            String sid = ck["sid"];
            //此客户已被踢出，可能因为有其他人用同一账号登陆
            if (LoginClientMgr_MVC.instance.isBan(sid))
            {
                Basic.trace(sid + " 此客户已被踢出，可能因为有其他人用同一账号登陆");
                LoginClientMgr_MVC.instance.Delete(sid);
                return Basic.StatusMessage.MSG_ANOTHER_LOGIN;
            }

            // 不在管理器login数组中的情况：此用户过长时间没有链接被删除
            if (!LoginClientMgr_MVC.instance.isLogin(sid))
            {
                Basic.trace(sid + " 过长时间没有链接已被删除");
                return Basic.StatusMessage.MSG_NOT_LOGIN;
            }

            ck = getCookie("heartbeat");
            //超时未心跳检测
            if (ck == null)
            {
                Basic.trace(sid + " 超时未心跳检测将被强制登出");
                LoginClientMgr_MVC.instance.Logout(sid);
                return Basic.StatusMessage.MSG_HBT_OVERTIME;
            }

            //验证通过，不在考试期间才会更新验证时间
            if (getCookie("test") == null)
            {
                ck.Expires = DateTime.Now.AddMinutes(Basic.HB_SPAN);
                LoginClientMgr_MVC.instance.heartBeat(sid);
            }
            return Basic.StatusMessage.MSG_VERIFY_SUCCESS;
        }

        public void testChosen(String name , int tl)
        {
            HttpCookie ck = new HttpCookie("test");
            ck.Value = HttpUtility.UrlEncode(name);//向cookie写入中文的值时先编码
            addCookie(ck);

            //考试时的心跳检测比考试限时多5分钟，可以在考试时断网
            ck = getCookie("heartbeat");
            ck.Expires = DateTime.Now.AddMinutes(tl + 5);
            addCookie(ck);
        }

        public void testOver()
        {
            HttpCookie ck = getCookie("test");
            if (ck != null)
            {
                ck.Expires = DateTime.Now.AddMinutes(-1);
                addCookie(ck);
            }
            ck = getCookie("heartbeat");
            if (ck != null)
            {
                ck.Expires = DateTime.Now.AddMinutes(Basic.HB_SPAN);
                addCookie(ck);
            }
        }

        public Boolean isOverTime()
        {
            if (getCookie("test") == null) return true; //没有在考试
            var ck = getCookie("heartbeat");
            if (ck == null) return true;
            //执行了试卷请求后的heartbeat cookie的生存时间是考试时间+5分钟
            TimeSpan t = ck.Expires - DateTime.Now;
            if (t.Minutes > 5) return true;
            return false;
        }
        /// <summary>
        /// 登出函数，客户端按下“注销”键时才会使用这个方法，将student cookie删除，
        /// 客户端直接退出应用删进程时，只能通过用户管理器的心跳检测每n个小时删除无回应用户。
        /// </summary>
        public override void logout()
        {
            HttpCookie ck = getCookie("student");
            try
            {
                LoginClientMgr_MVC.instance.Logout(ck["sid"]);
                ck.Value = null;
                ck.Expires = DateTime.Now.AddMinutes(-1);
                addCookie(ck);
            }
            catch (Exception err)
            {
                Basic.trace(err.Message);
            }
        }

        public String number
        {
            get { return getStudentProp("number"); }
        }

        public String SpecificID
        {
            get { return getStudentProp("sid"); }
        }

        public String testName
        {
            get
            {
                var tmp = getCookie("test");
                return HttpUtility.UrlDecode(tmp.Value);
            }
        }

        public String getStudentProp(String prop)
        {
            return getCkProp(prop, "student");
        }
    }
}
