package com.onlineclass.teacherside.net
{
	import com.onlineclass.info.AnswerInfo;
	import com.onlineclass.net.mvc.MVConnection;
	import flash.events.Event;
	import flash.net.URLRequest;
	import spark.components.Label;
	
	public class TeacherConMVC extends MVConnection implements ITSender
	{
		private var _type:String;
		private var _lab:Label;
		private var _ctrller:String;
		public function TeacherConMVC(lab:Label=null , type:String="POST")
		{
			super(type);
			if(lab) _lab=lab;
			if(_isGET) _ctrller = "Teacher";
			else _ctrller = "PTeacher";
			var u:String = "http://"+Basic.IP_HTTP+":"+String(Basic.PORT_MVC);
		}
		
		protected override function onComplete(e:Event):void{
			var obj:* = e.target.data.toString();
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
		public function testListRequest():void{
			_type = Basic.DATA_TEACHER;
			sendHttpRequest( "teacher" , "login" , _ctrller);
		}
		
		public function testRequest(test:String  , classs:String):void
		{
			_type = Basic.DATA_TSOCRE;
			var obj:Object=msgTrans( JSON.stringify( [test , classs] ) );
			sendHttpRequest( obj , "scoreRequest" , _ctrller);
		}
		public function classsRequest(testname:String):void{
			_type = Basic.DATA_TCLASS;
			var obj:Object=msgTrans(testname);
			sendHttpRequest(obj  , "classListRequest" ,_ctrller );
		}
		public function statRequest(testname:String):void{
			_type = Basic.DATA_STAT;	
			var obj:Object = msgTrans(testname);
			sendHttpRequest(obj , "statRequest" , _ctrller);
		}
		protected function msgTrans(msg:String):Object{
			var obj:Object;
			if(_isGET) obj={info: Base64Coder.instance.EncodeUTF8(msg)};
			else obj = msg;
			return obj;
		}
	}
}