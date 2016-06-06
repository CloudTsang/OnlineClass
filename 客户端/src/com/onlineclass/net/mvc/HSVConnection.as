package com.onlineclass.net.mvc
{
	import com.onlineclass.net.IConnection;
	import flash.events.EventDispatcher;
	import flash.events.IEventDispatcher;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.http.HTTPService;
	/**使用HttpService的网络连接*/
	public class HSVConnection extends EventDispatcher implements IConnection
	{
		protected var _svc:HTTPService;
		protected var _url:String;
		protected var _isGET:Boolean;
		protected var _switch:Boolean;
		public function HSVConnection(method:String="POST")
		{
			_svc = new HTTPService();
			if(method !="GET" && method!="POST") throw new Error("Wrong Param");
			_svc.method = method;
			_url = "http://"+Basic.IP_HTTP+":"+String(Basic.PORT_MVC)+"/";
			_svc.addEventListener(ResultEvent.RESULT , onComplete);
			_svc.addEventListener(FaultEvent.FAULT , errHandler);
			_switch=true;
		}
		protected function onComplete(e:ResultEvent):void{
			trace(e.result);
		}
		protected function errHandler(e:FaultEvent):void{
			trace(e.message);
		}
		
		/***
		 * 发送HTTP请求
		 * @param msg ：发送的信息
		 * @param act,ctrller,area ：控制器相关
		 * */
		protected function sendHttpRequest(msg:Object , act:String , ctrller:String , area:String=null):void{
			var tmp:String=_url;
			if(area) tmp = _url+area;
			tmp += ctrller+"/"+act;
			_svc.url = tmp;
			Send(msg);
		}
		
		public function set connectStatus(v:Boolean):void
		{
			if(!v) _svc.disconnect();
		}
		
		public function Send(message:Object):void
		{
			if(!_switch)return;
			_switch=false;
			_svc.send(message);
		}
	}
}