using System;
using System.Collections.Generic;
using MongoDB.Bson;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.db
{
    interface ITeacherDbCon
    {
        ///<summary>获取试卷testname有什么班级的成绩</summary>
        List<String> getClassList(String testname);
        ///<summary>获取成绩，返回有{name ， number ， score}3个属性的bsonArray</summary>
        BsonArray getScore(string testname, string classs);
        ///<summary>获取试卷testname的班级classs分数段信息 , 班级默认“all”为获取总人数统计结果</summary>
        IInfo getScoreNumber(String testname, String classs = "all");
        ///<summary>获取试卷testname分数统计结果</summary>
        List<StatInfo> getStat(String testname);

    }
}
