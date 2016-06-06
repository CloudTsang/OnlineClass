using System;
using System.Collections.Generic;
namespace TestOnlineMVC.tol.db
{
    interface IStudentDbCon
    {
        ///<summary>获取试题</summary>
        String getQuestion(String testname);     
        ///<summary>获取考试时间</summary>
        int getTestTime(String testname);
        ///<summary>获取试卷testname的答案</summary>
        List<int> getAnswer(String testname);
        ///<summary>设置testname试卷学号为number的学生的分数score,
        /// （这一方法将在数据库处进行班级查询，如果客户端改成会将班级信息也发送过来时，请使用 setScoreByClass)</summary>
        void setScore(string testname, string number, double score);
        ///<summary>设置testname试卷classs班学号为number的学生的分数score</summary>
        void setScoreByClass(string testname, string number, string classs, double score);
    }
}
