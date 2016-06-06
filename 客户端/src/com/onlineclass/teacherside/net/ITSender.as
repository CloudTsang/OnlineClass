package com.onlineclass.teacherside.net
{
	import com.onlineclass.net.IConnection;

	public interface ITSender extends IConnection
	{
		/**请求试卷列表**/
		function testListRequest():void;
		/**请求试卷test的classs班级分数**/
		function testRequest(test:String , classs:String):void;
		/**请求有testname试卷成绩的班级列表**/
		function classsRequest(testname:String):void;
		/**请求统计结果**/
		function statRequest(testname:String):void;	
	}
}