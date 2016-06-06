package com.onlineclass.net.sock 
{
	import com.codecatalyst.promise.Promise;
	import com.onlineclass.net.IHeartBeat;
	import flash.events.Event;
	import flash.events.ProgressEvent;
	import flash.events.TimerEvent;
	import flash.utils.Timer;
	import views.student.TestListView;
	/**心跳检测socket连接*/
	public class HBConnectionSO extends ServerConnection implements IHeartBeat
	{
		private var _timer:Timer;
		private const CLIENT:int = 0;
		private var _err:String;
		
		public function HBConnectionSO() 
		{
			super();
		}
		protected override function getData(e:ProgressEvent):void {
			_switch = true;
			var str:String = _socket.readMultiByte(_socket.bytesAvailable , SocketMessage.GB);
			trace("收到服务器的信息：" + str);
			if (str == "success") {
				trace("心跳检测成功");
				return;	
			}
			stopHB();
			_err = str;
			dispatchEvent( new Event(Basic.COMMU_ERROR) );
			trace("心跳检测失败");
		}
		
		protected function heartBeat(e:TimerEvent):void {
			trace("发送心跳包");
			var msg:String = SocketMessage.buildSoMsg(CLIENT , Basic.DATA_HBEAT , TestListView.sid.number , TestListView.sid.number);
			connectStatus=true;
			Send(msg);	
		}
		
		public function startHB(min:Number = 1):void {
			_timer = new Timer(min * 60 * 1000);
			trace("开始每隔 "+min+" 分钟向服务器发送1次心跳包");
			_timer.addEventListener(TimerEvent.TIMER , heartBeat);
			_timer.start();
		}
		public function stopHB():void {
			_timer.stop();	
		}
		
		public function get errMsg():String{
			return _err;
		}
	}
	
}