using System;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.db
{
    /// <summary> 数据库基本接口，实现学生侧和教师侧都要用到的功能：获取学生资料&试卷列表 </summary>
    interface IDbConBase
    {
        ///<summary>获取1个学号为no的学生资料</summary>
        StudentInfo getStudent(String no);
        ///<summary>获取试卷列表</summary>
        String getTestList();
        ///<summary>获取试卷testname在数据库中的key</summary>
        String getTestAddr(String testname);
        ///<summary>更新记录试卷名和数据库key的映射</summary>
        void renewTestNameTable();
    }
}
