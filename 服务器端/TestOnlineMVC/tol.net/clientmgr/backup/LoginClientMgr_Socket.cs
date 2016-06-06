using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.net.clientmgr
{
    public class LoginClientMgr_Socket : LoginClientMgr_Template
    {
        /// <summary>
        /// 登陆数组中超时的用户放到这个数组中，设置一段更长的过期时间，用于返回“连接已断开”的消息，再次过时之后会删除
        /// </summary>
        protected List<Client> _overTime = new List<Client>();
        /// <summary>
        /// 考试中学生的数组，考试超时的学生也会放在这里，但在再次心跳检测超时后会删除。
        /// </summary>
        protected List<Client_Student> _testStudent = new List<Client_Student>();

        protected new List<Client_Student> _list = new List<Client_Student>();

        #region 单例类
        private static LoginClientMgr_Socket _ins;
        public static void init() { if (_ins == null)_ins = new LoginClientMgr_Socket(); }
        public static LoginClientMgr_Socket instance
        {
            get { return _ins; }
        }
        #endregion

        public void testStart(String sid, String testname, int timelimit)
        {
            var tmp = searchBySid(sid, _list);
            if (tmp != null) tmp.testStart(testname, timelimit);
            _testStudent.Add(tmp);
            _list.Remove(tmp);
        }

        public void testOver(String sid)
        {
            var tmp = searchBySid(sid, _testStudent);
            if (tmp != null) tmp.testOver();
            _list.Add(tmp);
            _testStudent.Remove(tmp);
        }
        /// <summary>
        /// 用户sid是否正处于考试之中
        /// </summary>
        public String isTest(String sid)
        {
            var tmp = searchBySid(sid, _list);
            if (tmp == null) return String.Empty;
            return tmp.isTest;
        }
        ///<summary>用户sid考试是否已超时</summary>
        
        /// <summary>
        /// 用户sid是否已超时未做心跳检测
        /// </summary>
        public Boolean isOverTime(String sid)
        {
            var tmp = searchBySid(sid, _overTime);
            return tmp != null;
        }

        public override void Login(string num, string sid)
        {
            var tmp = searchByNumber(num, _list);
            if (tmp != null)
            {
                tmp.time = DateTime.Now.AddHours(5);
                _ban.Add(tmp);
                _list.Remove(tmp);
                Basic.trace(tmp.sid + " 被踢出。");
            }
            _list.Add(new Client_Student(num, sid));
            Basic.trace(sid + " 登陆了。");
        }

        public override void heartBeat(string sid)
        {
            searchBySid(sid, _list).time = DateTime.Now.AddMinutes(Basic.HB_SPAN);
            Basic.trace("更新 " + sid + " 检测时间");
        }

        public override void startHeartBeatTest()
        {
            _timer = new Timer(Basic.HB_SPAN * 60 * 1000);
            _timer.Elapsed += new ElapsedEventHandler(
               (object sender, ElapsedEventArgs args) =>
               {
                   Task t1 = new Task(overTimeTest);
                   Task.WaitAll(t1);
                   Task t2 = new Task(loginTest);

                   Task t3 = new Task(banTest);

               }
               );
            _timer.AutoReset = false;
            _timer.Start();
        }
        /// <summary>
        /// 已登陆的用户，超时未传来心跳包时放入overTime数组中
        /// </summary>
        private void loginTest()
        {
            foreach (var i in _list)
            {
                if (!i.isOnline())
                {
                    i.time = DateTime.Now.AddHours(1);
                    _overTime.Add(i);
                    _list.Remove(i);
                }
            }
        }
        
        /// <summary>
        /// 被踢出的用户会被设置一个很长的过期时间，过期后完全删除，
        /// 在这之前该用户发来请求时，将在返回  登陆异常  的信息后将其删除
        /// </summary>
        private void banTest()
        {
            foreach (var j in _ban)
            {
                if (!j.isOnline()) _ban.Remove(j);
                
            }
        }

        /// <summary>
        /// 检测超时的用户会被设置一个稍长的过期时间，过期后完全删除，
        /// 在这之前该用户发来请求时，将在返回  连接已断开  的信息后将其删除
        /// </summary>
        private void overTimeTest()
        {
            foreach (var k in _overTime)
            {
                if (!k.isOnline()) _overTime.Remove(k);
            }
        }

        /// <summary>
        /// 考试用户检测，有三种情况：
        /// <remarks>正在考试中，检测时间为考试时间：正常情况;</remarks>
        /// <remarks>考试已结束，检测时间为一般心跳检测间隔：0分情况</remarks>
        /// <remarks>考试已结束，检测已超时：删除</remarks>
        /// </summary>
        private void testTest()
        {
            foreach (var l in _testStudent)
            {
                if (!l.isOnline())
                {
                    if (l.isTest != String.Empty)
                    {
                        l.testOver();
                    }
                    else
                    {
                        
                    }
                }
            }
        }
    }
}