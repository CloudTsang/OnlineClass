using System;
using System.Collections.Generic;
using TestOnlineMVC.tol.info;
namespace TestOnlineMVC.tol.net
{
	/// <summary>学生请求处理</summary>
	public interface IStudentEvt
	{
		///<summary>登陆处理，登陆失败时返回null，成功时返回该学生的信息，sendBack设为学生信息和试卷列表的json</summary>
		IInfo loginHandler(String info);
		///<summary>获取试卷题目,返回试卷的"试卷名-限制时间"这样的键值对</summary>
		KeyValuePair<String , int> getQuestion(String testname);
		///<summary>批改试卷分数，返回分数</summary>
        double getScore(String ans, String testname, String num);
		///<summary>试卷0分</summary>
		void Score0(String testname , String num);
        /// <summary>放弃考试</summary>
	    void giveup();
        /// <summary>登出</summary>
	    void logout();
		///<summary>获取要发送给客户端的字符串</summary>
		String sendBack{get;}
		
	}
}
