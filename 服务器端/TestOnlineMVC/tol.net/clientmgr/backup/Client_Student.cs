using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOnlineMVC.tol.net.clientmgr
{
    public class Client_Student:Client_withHB
    {
        protected Boolean _isTest;
        protected String _test;
        public const int STATUS_TESTING = 3;
        public const int STATUS_TEST_OVERTIME = 4;
        public Client_Student(string n, string id) : base(n, id)
        {
            _test = String.Empty;
            _isTest = false;
        }
        /// <summary>
        /// 开始考试，设置考试限时min
        /// </summary>
        public void testStart(String testname , int timelimit)
        {
            _status = STATUS_TESTING;
            _test = testname;
            _isTest = true;
            _hb = DateTime.Now.AddMinutes(timelimit + 5);
        }
        /// <summary>
        /// 结束考试，将用户过期时间设置为心跳检测间隔.
        /// </summary>
        public void testOver()
        {
            _status = STATUS_ONLINE;
            _isTest = false;
            _hb = DateTime.Now.AddMinutes(Basic.HB_SPAN);
        }
        /// <summary>
        /// 该学生是否正在考试,是的场合放回试卷名称
        /// </summary>
        public String isTest
        {
            get { return _test; }
        }

        public override Boolean isOnline()
        {
            TimeSpan span = DateTime.Now - _hb;
            if (span.Minutes < 0)
            {
                if (_status==STATUS_TESTING)
                {
                    /**考试中检测到超时的场合将过期时间设置回一般心跳间隔，退出考试状态。
                     **/
                    _hb = DateTime.Now.AddMinutes(Basic.HB_SPAN);
                    _status = STATUS_TEST_OVERTIME;
                }
                else if (_status == STATUS_ONLINE)
                {
                    _status = STATUS_TEST_OVERTIME;
                    _hb = DateTime.Now.AddHours(1);
                }
                return false; 
            }
            if (_status==STATUS_ONLINE) _hb = DateTime.Now.AddMinutes(Basic.HB_SPAN); //只在考试期间不需要进行心跳检测时更新过期时间
            return true;
        }
    }
}