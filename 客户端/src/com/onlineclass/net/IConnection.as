package com.onlineclass.net
{
	import flash.events.Event;
	import flash.events.IEventDispatcher;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;

	public interface IConnection  extends IEventDispatcher
	{
		/**连接状态，设为true会进行socket连接，设为false会断开**/
		function set connectStatus(v:Boolean):void;
		/**发送socket信息，在这个应用中，发送的信息只能是以下两种：登陆信息json，试题答案序列*/
		function Send(message:Object):void;
	}
}