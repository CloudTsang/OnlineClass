package com.onlineclass.teacherside.net
{
	import com.onlineclass.info.AnswerInfo;
	import com.onlineclass.net.ISender;
	import com.onlineclass.net.sock.ServerConnection;
	import com.onlineclass.net.sock.SocketMessage;
	
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.events.TimerEvent;
	import flash.utils.Timer;
	
	public class TeacherCon extends ServerConnection implements ITSender
	{
		private const CLIENT:int = 1;
		private const ACC:String ="*";
		private var _type:int;
		private var _coder64:Base64Coder;
		public function TeacherCon()
		{
			super();
			_coder64 = new Base64Coder();
		}
		
		protected override function getData(e:ProgressEvent):void{
//			var str:String = _socket.readUTFBytes(_socket.bytesAvailable);
			var obj:* = _socket.readMultiByte(_socket.bytesAvailable , SocketMessage.GB);
			try{
				obj = Base64Coder.instance.Decode(obj);
				trace("来自服务器 : "+obj);
				obj  =JSON.parse(obj);
			}catch(err:Error){
				trace(err.getStackTrace());
				_type=Basic.DATA_ERR;
			}
			_switch=true;
			DataProcessor.instance.convertToInst( obj , _type);
			dispatchEvent( new Event(Basic.COMMUNICATE_COMPLETE) );
		}

		public function testListRequest():void{
			_type = Basic.DATA_TEACHER;
			var msg:String = SocketMessage.buildSoMsg( CLIENT , _type , ACC , "teacherlogin" );
			connectStatus = true;
			Send( msg );
		}
		
		protected override function errHandler(e:IOErrorEvent):void{
			super.errHandler(e);
			DataProcessor.instance.convertToInst(e.text , Basic.DATA_ERR);
			dispatchEvent( new Event(Basic.COMMUNICATE_COMPLETE) );
		}
		public function testRequest(test:String , classs:String):void{
			_type = Basic.DATA_TSOCRE;
			var str:String = JSON.stringify( [ test  , classs ] );
			var msg:String = SocketMessage.buildSoMsg( CLIENT , _type , ACC , str );
			connectStatus = true;
			Send(msg);
		}

		public function classsRequest(testname:String):void{
			_type = Basic.DATA_TCLASS;
			var msg:String = SocketMessage.buildSoMsg( CLIENT , _type , ACC , testname);
			connectStatus = true;
			Send(msg);
		}
		public function statRequest(testname:String):void{
			_type = Basic.DATA_STAT;
			var msg:String = SocketMessage.buildSoMsg( CLIENT , _type , ACC , testname);
			connectStatus = true;
			Send(msg);
		}
		public function logout():void{
			connectStatus=false;
		}
	}
}