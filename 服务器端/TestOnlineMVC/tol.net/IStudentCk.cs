using System;
namespace TestOnlineMVC.tol.net
{
	/// <summary> 操作学生端cookie </summary>
	public interface IStudentCk
	{
		///<summary>记录学生的考试信息,并且将心跳检测cookie过期时间延长至试卷限时tl+5分钟</summary>
		void testChosen(String name , int tl);
        ///<summary>学生考试结束，删除记录考试信息的cookie,心跳检测cookie过期时间设置回普通间隔</summary>
	    void testOver();
		///<summary>学生考试是否已经超时, 已超时返回true</summary>
		Boolean isOverTime();
		///<summary>学生登出,删除cookie</summary>
		void logout();
		///<summary>发来请求的学生的学号</summary>
		String number{get;}
		///<summary>学生正在做的试卷名</summary>
		String testName{get;}
	}
}
