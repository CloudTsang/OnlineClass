/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/3/23
 * 时间: 21:29
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
namespace TestOnlineMVC.tol.net
{
	/// <summary>
	/// 登陆的学生列表
	/// </summary>
	public class LoginClient
	{
		private static Hashtable _clients = new Hashtable();
		/**加入学号为num的学生及id*/
		public static void Add( String num , String sessionid ){
			_clients. Add(num , sessionid);
		}
		/**删除学号为num的学生*/
		public static void Remove(String num){
			_clients.Remove(num);
		}
		/**获得学号为num的学生的id*/
		public static String getSessionID(String num){
			return _clients[num] as String;
		}
	}
}
