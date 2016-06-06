using System;
using Newtonsoft.Json;
namespace TestOnlineMVC.tol.info
{
	/// <summary>学生信息</summary>
	public class StudentInfo : TeacherInfo
	{
		public String classs;
		
		public StudentInfo(){		}

	    public StudentInfo(String no, String na, String cl, String pw)
	    {
	        number = no;
	        name = na;
	        classs = cl;
	        password = pw;
	    }
		
		///<summary>学生信息:基础，用于发送给客户端，屏蔽了登录密码</summary>
		[JsonIgnore]
		public StudentInfo ClientSide{
			get{
			    return new StudentInfo(number , name ,classs , "********");
			}
		}
	}
}
