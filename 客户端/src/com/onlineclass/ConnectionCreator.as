package com.onlineclass
{
	import com.onlineclass.net.IConnection;
	import com.onlineclass.net.IHeartBeat;
	import com.onlineclass.net.ISender;
	import com.onlineclass.net.mvc.HeartBeatConnection;
	import com.onlineclass.net.mvc.TestolConnectionHSVC;
	import com.onlineclass.net.mvc.TestolConnectionMVC;
	import com.onlineclass.net.sock.HBConnectionSO;
	import com.onlineclass.net.sock.TestolConnection;
	import com.onlineclass.teacherside.net.TeacherCon;
	import com.onlineclass.teacherside.net.TeacherConHSVC;
	
	import spark.components.Label;
	import spark.effects.animation.MotionPath;
	
	public class ConnectionCreator
	{
		public static function create(client:String , method:String , txtTips:Label):void
		{
			if(client=="student"){
				switch(method)
				{
					case "Socket":
						StudentSide.con = new TestolConnection(txtTips);
						StudentSide.hbcon = new HBConnectionSO();
						trace("本测试使用socket连接进行");
						return;
					case "POST":
						StudentSide.con = new TestolConnectionMVC(txtTips , "POST");
						trace("本测试使用POST请求进行");
						break;
					case "GET":
						StudentSide.con = new TestolConnectionHSVC(txtTips , "GET");
						trace("本测试使用GET请求进行");
						break;
				}
				if(!StudentSide.hbcon) StudentSide.hbcon = new HeartBeatConnection("student");
			}
			else if(client=="teacher"){
				switch( method )
				{
					case "Socket":
						TeacherSide.con = new TeacherCon();
						break;
					case "POST":
						TeacherSide.con = new TeacherConHSVC(txtTips);
						break;
					case "GET":
						TeacherSide.con = new TeacherConHSVC(txtTips , "GET");
						break;
				}
			}
		}
	}
}