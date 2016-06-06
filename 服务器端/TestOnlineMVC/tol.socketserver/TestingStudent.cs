using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOnlineMVC.tol.socketserver
{
    /**参加考试得学生的信息记录在这里*/
    public class TestingStudent
    {
        private static List<TestingStudent> list = new List<TestingStudent>();
        public String number;
        public String test;
        public DateTime endTime;

        public TestingStudent(String n, String t , int tl)
        {
            number = n;
            test = t;
            setEnd(tl);
        }

        protected void setEnd(int min)
        {
            endTime = DateTime.Now.AddMinutes(min);
        }

        public static void Add(String n, String t , int tl)
        {
            var i = search(n);
            if (i != null)
            {
                i.test = t;
                i.setEnd(tl);
                return;
            }
            list.Add( new TestingStudent(n,t ,tl) );
        }

        public static void Remove(TestingStudent s)
        {
            list.Remove(s);
        }

        public static Boolean isOverTime(TestingStudent s)
        {
//            var student = list.Find((s) => { return s.number == n; });
            TimeSpan t = s.endTime - DateTime.Now;
            if (t.Minutes > 0) return false;
            return true;
        }

        public static TestingStudent search(String n)
        {
            return list.Find((s) => { return s.number == n; });
        }

    }
}