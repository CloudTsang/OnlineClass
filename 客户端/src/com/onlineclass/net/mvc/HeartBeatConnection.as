package com.onlineclass.net.mvc
{
	import com.onlineclass.net.IHeartBeat;
	
	import flash.events.Event;
	import flash.events.TimerEvent;
	import flash.utils.Timer;
	
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	
	import spark.components.Label;
	
	/**进行心跳检测的MVC链接*/
	public class HeartBeatConnection extends HSVConnection implements IHeartBeat
	{
		private var _timer:Timer;
		private var _act:String;
		public var _err:String;
		public function HeartBeatConnection(client:String="student"  ,method:String="POST" )
		{
			super(method);
			if(client!="student" && client!="teacher") throw new Error("Wrong param");
			_act = client;
			
			_url = "http://"+Basic.IP_HTTP+":"+String(Basic.PORT_MVC)+"/";
			_svc.addEventListener(ResultEvent.RESULT , onComplete);
			_svc.addEventListener(FaultEvent.FAULT , errHandler);
		}
		protected override function onComplete(e:ResultEvent):void{
			_switch=true;
			var str:String = e.result as String;
			trace(str);
			if(str == "success") {
				trace("心跳检测成功");
				return;
			}
			stopHB();
			_err = str;
			dispatchEvent( new Event(Basic.COMMU_ERROR) );
			trace("心跳检测失败");
		}
		
		protected function heartBeat(e:TimerEvent):void{
			trace("进行心跳检测");
			try{
				sendHttpRequest(null , _act , "heartBeat");
			}catch(err:Error){
				trace(err.message);
			}
		}
		public function startHB(min:Number=1):void{
			_timer = new Timer(min * 60 * 1000);
			trace("开始每隔 "+min+" 分钟向服务器发送1次心跳包");
			_timer.addEventListener(TimerEvent.TIMER , heartBeat);
			_timer.start();
		}
		public function stopHB():void{
			_timer.stop();
		}
		public function get errMsg():String{
			return _err;
		}
	}
}