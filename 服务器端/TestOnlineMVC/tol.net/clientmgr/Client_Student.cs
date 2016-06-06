using System;
namespace TestOnlineMVC.tol.net.clientmgr
{
    public class Client_Student:Client
    {
        protected Boolean _isTest;
        protected String _test;
        /// <summary>考试中状态</summary>
        public const int STATUS_TESTING = 3;
        /// <summary>考试超时未交卷状态（0分）</summary>
        public const int STATUS_TEST_OVERTIME = 4;
        public Client_Student(string n, string id) : base(n, id)
        {
            _test = String.Empty;
            _isTest = false;
        }

        /// <summary> 开始考试，设置考试限时(min) </summary>
        public void testStart(String testname , int timelimit)
        {
            _status = STATUS_TESTING;
            _test = testname;
            _hb = DateTime.Now.AddMinutes(timelimit + 5);
        }

        /// <summary> 结束考试，将用户过期时间设置为心跳检测间隔. </summary>
        public void testOver()
        {
            _status = STATUS_ONLINE;
            _test = String.Empty;
            _hb = DateTime.Now.AddMinutes(Basic.HB_SPAN);
        }
        /// <summary> 设置考试超时状态,此状态下传来答案的场合仍能获取试卷名称，但是分数为0分 </summary>
        public void testOverTime()
        {
            _status = STATUS_TEST_OVERTIME;
            _hb = DateTime.Now.AddMinutes(Basic.HB_SPAN);
        }
        public override void overTime()
        {
            _test = String.Empty;
            base.overTime();
        }
        /// <summary> 该学生是否正在考试,是的场合返回试卷名称 </summary>
        public String isTest
        {
            get { return _test; }
        }
    }
}