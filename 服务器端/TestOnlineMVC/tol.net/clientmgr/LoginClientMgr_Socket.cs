using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace TestOnlineMVC.tol.net.clientmgr
{
    public class LoginClientMgr_Socket : LoginClientMgr_Template
    {
        #region 单例类
        private static LoginClientMgr_Socket _ins;
        public static void init() { if (_ins == null) _ins = new LoginClientMgr_Socket(); }
        public static LoginClientMgr_Socket instance
        {
            get { return _ins; }
        }
        #endregion

        #region 考试相关
        public void testStart(String sid, String testname, int timelimit)
        {
            var tmp = searchBySid<Client_Student>(sid);
            if (tmp != null)
            {
                tmp.testStart(testname, timelimit);
                tmp.status = Client_Student.STATUS_TESTING;
            }
        }

        public void testOver(String sid)
        {
            var tmp = searchBySid<Client_Student>(sid);
            if (tmp != null) tmp.testOver();
        }

        /// <summary> 用户sid是否正处于考试之中 </summary>
        /// <returns> 
        /// 1个键值对，key是试卷名称，没有在考试得场合为String.Empty是的场合返回考试名称，不是的场合返回String.Empty
        /// value是考试是否超时
        /// </returns>
        public KeyValuePair<String, Boolean> testStatus(String sid)
        {
            var tmp = searchBySid<Client_Student>(sid);
            String name = tmp.isTest;
            Boolean ot = (tmp.status == Client_Student.STATUS_TEST_OVERTIME);
            return new KeyValuePair<string, bool>(name, ot);
        }

        #endregion

        public override void Login(string num, string sid)
        {
            Add<Client_Student>(num, sid);
        }

        public override bool isLogin(string sid)
        {
            var tmp = searchBySid<Client_Student>(sid);
            if (tmp == null) return false;
            return tmp.status == Client.STATUS_ONLINE || tmp.status == Client_Student.STATUS_TESTING;
        }

        /// <summary> 用户sid是否已超时未做心跳检测 </summary>
        public Boolean isOverTime(String sid)
        {
            var tmp = searchBySid<Client_Student>(sid);
            return tmp != null && tmp.status == Client_Student.STATUS_OVERTIME;
        }

        public override void heartBeat(string sid)
        {
            var tmp = searchBySid<Client_Student>(sid);
            if (tmp.status == Client_Student.STATUS_TESTING) return;
            Basic.trace("更新 " + sid + " 检测时间");
            tmp.time = DateTime.Now.AddMinutes(Basic.HB_SPAN);
        }

        public override void startHeartBeatTest()
        {
            _timer = new Timer(Basic.HB_SPAN * 60 * 1000);
            _timer.Elapsed += new ElapsedEventHandler(
               (object sender, ElapsedEventArgs args) =>
               {
                   Task t = new Task(clientTest);
                   t.Start();
               }
               );
            _timer.AutoReset = true;
            _timer.Start();
        }

        /// <summary>
        /// 检测用户登录情况：
        /// 1:已登陆的用户，超时未传来心跳包时放入overTime数组中;
        /// 2:被踢出的用户会被设置一个很长的过期时间，过期后完全删除，
        /// 在这之前该用户发来请求时，将在返回  登陆异常  的信息后将其删除;
        /// 3:检测超时的用户会被设置一个稍长的过期时间，过期后完全删除，
        /// 在这之前该用户发来请求时，将在返回  连接已断开  的信息后将其删除;
        /// 4:考试用户检测，有三种情况：
        /// <remarks>正在考试中，检测时间为考试时间：正常情况 isTest==true,isOnline==true;</remarks>
        /// <remarks>考试已结束，检测时间为一般心跳检测间隔：0分情况 isTest==true,isOnline==false</remarks>
        /// <remarks>考试已结束，检测已超时：删除 isTest==false,isOnline==false</remarks>
        ///  </summary>
        protected void clientTest()
        {
//            Basic.trace("开始扫描登录用户的在线情况");
            foreach (var i in _list)
            {
                if (!i.isOnline())
                {
                    switch (i.status)
                    {
                        //正常在线中→过时→超时未心跳检测
                        case Client_Student.STATUS_ONLINE:
                        //考试超时→再次过期→超时未心跳检测
                        case Client_Student.STATUS_TEST_OVERTIME:
                            Basic.trace(i.sid+" 未及时检测，设置为超时组");
                            i.overTime();
                            break;
                        //检测过期后再次过期→删除 
                        case Client_Student.STATUS_OVERTIME:
                        // 被踢出后过期→删除
                        case Client_Student.STATUS_BAN:
                            Basic.trace(i.sid+" 用户已过期，将其删除");
                            _list.Remove(i);
                            break;
                        //正在考试中→过期→考试超时
                        case Client_Student.STATUS_TESTING:
                            Basic.trace(i.sid+" 考试超时，设为0分组");
                            ((Client_Student)i).testOverTime();
                            break;
                        
                    }
                }
            }
//            Basic.trace("扫描结束");
        }
    }
}