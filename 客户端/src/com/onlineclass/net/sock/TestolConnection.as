package com.onlineclass.net.sock
{
	import com.onlineclass.info.AnswerInfo;
	import com.onlineclass.info.LoginInfo;
	import com.onlineclass.net.ISender;
	
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	
	import mx.charts.chartClasses.DataDescription;
	import mx.states.OverrideBase;
	
	import spark.components.Label;
	
	import views.student.TestListView;

	public class TestolConnection extends ServerConnection implements ISender
	{
		private var _lab:Label;
		private var _type:int;
		private const CLIENT:int = 0;
		public function TestolConnection(txt:Label=null)
		{
			super();
			_lab=txt;
		}

		protected override function getData(e:ProgressEvent):void{
//			var str:String = _socket.readUTFBytes(_socket.bytesAvailable);
			var obj:*= _socket.readMultiByte(_socket.bytesAvailable , SocketMessage.GB);
			try{
				obj = Base64Coder.instance.Decode(obj);
				trace("来自服务器 : "+obj);
				obj  =JSON.parse(obj);
			}catch(err:Error){
				trace(err.getStackTrace());
				_type=Basic.DATA_ERR;
			}
			DataProcessor.instance.convertToInst( obj , _type);
			_switch=true;
			if(_type == Basic.DATA_LOGOUT)return;
			dispatchEvent( new Event(Basic.COMMUNICATE_COMPLETE) );
		}
		protected override function errHandler(e:IOErrorEvent):void{
			super.errHandler(e);
			DataProcessor.instance.convertToInst(e.text , Basic.DATA_ERR);
			dispatchEvent( new Event(Basic.COMMUNICATE_COMPLETE) );
		}
		
		public function login(no:String , pass:String):void{
			if(_lab)_lab.text="正在验证登陆信息，请稍候...";
			var msg:String = SocketMessage.buildSoMsg( CLIENT , Basic.DATA_LOGIN , no , LoginInfo.convertToJson(no,pass) );
			connectStatus=true; 
			Send( msg );
			_type = Basic.DATA_LOGINSUCCESS;
		}
		
		public function testRequest( test:String):void{
			if(_lab)_lab.text="正在向服务器请求试题，请稍候...";
			var msg:String = SocketMessage.buildSoMsg( CLIENT , Basic.DATA_REQUEST , TestListView.sid.number , test );
			connectStatus=true;
			Send( msg );
			_type = Basic.DATA_QUESTIONS;
		}
		
		public function sendAnswer( ans:AnswerInfo ):void{
			if(_lab)_lab.text="正在等待服务器批改答案，请稍候...";
			var str:String = ans.convertToJson_AnsOnly;
			trace("发送答案："+str);
			var msg:String = SocketMessage.buildSoMsg( CLIENT , Basic.DATA_ANSWERS , TestListView.sid.number , str );
			connectStatus=true;
			Send( msg );
			_type = Basic.DATA_SCORE;
		}
		public function giveup(test:String):void{
			if(_lab)_lab.text="正在请求中止本次测验，请稍后...";
			var msg:String = SocketMessage.buildSoMsg(CLIENT , Basic.DATA_GIVEUP , TestListView.sid.number , test);
			connectStatus=true;
			Send(msg);
			_type = Basic.DATA_GIVEUP;
		}
		
		public function logout():void{
			var msg:String = SocketMessage.buildSoMsg(CLIENT , Basic.DATA_LOGOUT , TestListView.sid.number , TestListView.sid.number);
			connectStatus=true;
			Send(msg);
			_type = Basic.DATA_LOGOUT;
		}
	}
}