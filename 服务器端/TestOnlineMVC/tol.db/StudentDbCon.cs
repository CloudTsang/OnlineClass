using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace TestOnlineMVC.tol.db
{
    public class StudentDbCon : DbConBase , IStudentDbCon
    {
        private static  StudentDbCon _ins;
        public StudentDbCon(String mdb="db1", String mcom = MONGO_CONNECT_STRING, String rhost = DB_ADDR, String rpw = DB_PASSWORD)
            : base(mdb,mcom ,rhost,rpw)
        {
        }

        public static StudentDbCon instance
        {
            get
            {
                if(_ins == null) _ins = new StudentDbCon();
                return _ins;
            }
        }

        ///<summary>获取试题的json字符串（服务器端无需解析该json，获取字符串后直接发送给客户端即可）</summary>
        public string getQuestion(string testname)
        {
            Basic.trace("从数据库获取试卷 " + testname);
            return rcon.Get("QuestionLib:" + getTestAddr(testname));
        }

        public int getTestTime(string testname)
        {
            Basic.trace("从数据库获取试卷 "+testname+" 的考试限时");
            var time = rcon.HGet("TestTime", getTestAddr(testname));
            return int.Parse(time);
        }

        public List<int> getAnswer(string testname)
        {
            Basic.trace("从数据库获取试卷 " + testname + " 的答案");
            String result = rcon.Get("AnswerLib:" + getTestAddr(testname));
            return JsonConvert.DeserializeObject<List<int>>(result);
        }

        public void setScore(string testname, string number, double score)
        {
            var cli = getStudent(number);
            setScoreByClass(testname , cli.number , cli.classs , score);
        }

        public void setScoreByClass(string testname, string number, string classs, double score)
        {
            var name = getTestAddr(testname);
            var col = mcon.GetCollection("Score:" + name);
            if (!col.Exists())//没有这个集合时新建&索引
            {
                Basic.trace(testname + " 试卷的第一名上交学生。");
                var str = BsonJavaScript.Create("addIndexForScore(" + name.ToJson() + ")" );
                mcon.Eval(EvalFlags.None, str);
//                使用C#驱动建立索引
//                var indexKey = new IndexKeysBuilder();
//                indexKey.Ascending("score");
//                var indexOption = new IndexOptionsBuilder();
//                col.CreateIndex(indexKey, indexOption);
            }
            col.Save
                (
                new BsonDocument
		        {
		            {"_id", number},
                    {"classs",classs},
		            {"score", score}
		        }
                );
            DbBasic.rwHt[name] = DbBasic.STAGE_WRITE;
        }
    }
}