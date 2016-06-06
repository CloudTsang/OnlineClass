using System;
using TestOnlineMVC.tol.db;
using TestOnlineMVC.tol.info;
using Newtonsoft.Json;
using System.Collections.Generic;
using TestOnlineMVC.tol.coder;

namespace TestOnlineMVC.tol.net
{
    /// <summary> 处理MVC学生端请求 </summary>
    public class StudentEvtHandler : IStudentEvt
    {
        protected StudentDbCon _dbcon;
        protected String _back;
        public StudentEvtHandler(StudentDbCon db = null)
        {
            if (db == null) _dbcon = StudentDbCon.instance;
            else _dbcon = db;
        }
        public virtual IInfo loginHandler(String info)
        {
            Basic.trace(info);
            StudentInfo ser=loginVerify(info) as StudentInfo;
            if (ser != null)
            {
                string[] arr =
                {
                    ser.ClientSide.convertToJson(),  //用户详细信息
                    _dbcon.getTestList(),  //试卷列表
                    Basic.HB_SPAN.ToString()  //心跳检测间隔
                };
                _back = JsonConvert.SerializeObject(arr);
            }
            return ser;
        }

        protected IInfo loginVerify(String info)
        {
            LoginInfo cli;
            StudentInfo student;
            try
            {
                cli = JsonConvert.DeserializeObject<LoginInfo>(info);
                student = _dbcon.getStudent(cli.number);
            }
            catch (Exception)
            {
                _back = "信息传输错误";
                return null;
            }
            if (student == null || student.password != cli.password)
            {
                _back = "账号或密码错误。";
                return null;
            }
            return student;
        }

        public virtual KeyValuePair<String , int> getQuestion(String testname)
        {
            int time = _dbcon.getTestTime(testname);
            String ques = _dbcon.getQuestion(testname);
            if (time == 0 || ques == null)
            {
                _back = "信息传输错误";
                return new KeyValuePair<string, int>(String.Empty,0);
            }
            _back = ques;
            return new KeyValuePair<string, int>(testname, time);
        }

        public virtual double getScore(String ans, String testname, String num)
        {
            List<int> cli = JsonConvert.DeserializeObject<List<int>>(ans);
            double sc = AnswerInfo.check2(cli,
                                       testname,//读取cookie中文的值时先解码
                                       _dbcon);
            _dbcon.setScore(testname, num, sc);
            _back = sc.ToString();
            return sc;
        }

        public virtual void Score0(String testname, String num)
        {
            _dbcon.setScore(testname, num, 0);
            _back = "0";
        }

        public virtual void giveup()
        {
            //暂时没有需要拒绝中断考试的情况
            _back = "1";
        }

        public void logout()
        {
            //暂时没有需要拒绝登出的情况
            _back = "1";
        }

        public String sendBack
        {
            get
            {
                return _back;
            }
        }
    }
}
