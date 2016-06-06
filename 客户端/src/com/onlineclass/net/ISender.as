package com.onlineclass.net
{
	import com.onlineclass.info.AnswerInfo;
/**发送信息给服务器的接口**/
	public interface ISender extends IConnection
	{
		/**发送登陆信息**/
		function login(no:String , pass:String):void;
		/**发送试卷请求**/
		function testRequest( test:String):void;
		/**发送答案**/
		function sendAnswer( ans:AnswerInfo ):void;
		/**放弃考试*/
		function giveup(test:String):void;
		/**发送登出信息**/
		function logout():void;
	}
}