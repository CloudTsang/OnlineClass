package
{
	public class Basic
	{
                /**ip地址*/public static var IP_SO:String="192.168.24.1";
//                /**ip地址*/public static var IP_SO:String="10.10.103.8";
		/**ip地址*/public static var IP_HTTP:String="localhost";
//		/**ip地址*/public static var IP_HTTP:String="192.168.24.1";
		/**Socket端口*/public static const PORT:int=9985;
		/**MVC端口*/public static const PORT_MVC:int = 7841;

		//学生端
		/**登录口令**/public static const DATA_LOGIN:int = 0;
		/**登陆成功信息**/public static const DATA_LOGINSUCCESS:int=1;
		/**用户资料*/public static const DATA_STUDENT:int=2;
		/**答案**/public static const DATA_ANSWERS:int=3;
		/**题目*/public static const DATA_QUESTIONS:int=4;
		/**分数**/public static const DATA_SCORE:int = 5;
		/**放弃作答*/public static const DATA_GIVEUP:int=14;
		/**心跳检测返回信息**/public static const DATA_HBEAT:int=15;
		/**登出**/public static const DATA_LOGOUT:int=16;
		
		//教师端
		/**教师端登录**/public static const DATA_TEACHER:int=6;
		/**试题列表*/public static const DATA_TESTLIST:int=7;
		/**试卷班级列表**/public static const DATA_TCLASS:int = 8;
		/**教师端分数信息**/ public static const DATA_TSOCRE:int = 9;
		/**统计信息*/public static const DATA_STAT:int =10;
		/**分数段人数统计信息*/public static const DATA_CHART:int = 11;

		/**其他请求**/public static const DATA_REQUEST:int=12;
		/**错误信息**/public static const DATA_ERR:int = 13;
		
		/**事件：提示框确认*/public static const ALERT_CONFIRM:String="alertconfirm";
		/**事件：心跳检测失败**/public static const COMMU_ERROR:String="heartbeaterror";
		/**事件 : 通信完毕**/public static const COMMUNICATE_COMPLETE:String="socketfinish";
	}
}