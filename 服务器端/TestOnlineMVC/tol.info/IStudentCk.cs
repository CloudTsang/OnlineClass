/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/3/23
 * 时间: 20:55
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */

using System;

namespace TestOnlineMVC.tol.info
{
	/// <summary>
	/// 操作学生端cookie
	/// </summary>
	public interface IStudentCk
	{
		/**登陆成功,生成cookie*/
		void login(StudentInfo student);
		/**账户验证*/
		Boolean verify();
		/**记录学生的考试信息 , 参数字符串必须是 “{试卷名}&&{限制时间}” 这样的格式*/
		void testChosen(String str);
		/**学生考试是否已经超时, 已超时返回true*/
		Boolean isOverTime();
		/**学生登出,删除cookie*/
		void logout();
		/**发来请求的学生的学号*/
		String number{get;}
		/**学生正在做的试卷名*/
		String testName{get;}
		/**获取学生cookie的子键prop的值*/
		String getCkProp(String prop);
	}
}
