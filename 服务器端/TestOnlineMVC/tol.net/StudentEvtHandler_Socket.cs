using System;
using System.Text;
using Newtonsoft.Json;
using TestOnlineMVC.tol.coder;
using TestOnlineMVC.tol.db;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.net
{
    public class StudentEvtHandler_Socket : StudentEvtHandler
    {
        public StudentEvtHandler_Socket(StudentDbCon db = null, ICoder co = null)
        {
        }
        /// <summary> socket侧使用的登陆方法，返回信息中增加了用户特征码 </summary>
        public override IInfo loginHandler(string info)
        {
            Basic.trace(info); 
            StudentInfo ser = loginVerify(info) as StudentInfo;
            String sid = createStr(new[] {ser.number, DateTime.Now.ToString()});
            if (ser != null)
            {
                string[] arr = { 
                                   ser.ClientSide.convertToJson(),  //学生信息
                                   _dbcon.getTestList(),   //试卷列表
                                   Basic.HB_SPAN.ToString() , //心跳检测间隔
                                   sid //特征码 ， 目前仅使用“账号（连接）登陆时间”的字符串
                               };
                _back = JsonConvert.SerializeObject(arr);
            }
            return new ClientInfo(ser.number , sid);
        }

        public override double getScore(string ans, string testname, string num)
        {
            if (testname == String.Empty)
            {
                _back = "考试信息已丢失";
                return 0;
            }
            return base.getScore(ans, testname, num);
        }

        protected String createStr(String[] arr)
        {
            StringBuilder str = new StringBuilder();
            foreach (var i in arr)
            {
                str.Append(i);
            }
            return str.ToString();
        }
    }
}