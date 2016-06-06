package com.onlineclass.net
{
	public interface IHeartBeat extends IConnection
	{
		/**开始每个min分钟发送心跳包**/
		function startHB(min:Number=1):void;
		/**停止发送心跳包**/
		function stopHB():void
		function get errMsg():String	
	}
}