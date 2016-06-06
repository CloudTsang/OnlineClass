package com.onlineclass.teacherside.net
{
	import com.onlineclass.info.AnswerInfo;
	import com.onlineclass.net.mvc.HSVConnection;
	import flash.events.Event;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.http.HTTPService;
	import spark.components.Label;
	
	public class TeacherConHSVC extends HSVConnection implements ITSender
	{
		private var _type:int;
		private var _lab:Label;
		private var _ctrller:String;
		public function TeacherConHSVC(lab:Label , method:String="POST")
		{
			super(method);
			if(lab) _lab=lab;
			if(method =="GET") {
				_isGET=true;
				_ctrller = "Teacher";
			}
			else {
				_isGET=false;
				_ctrller = "PTeacher";
				_svc.contentType=HTTPService.CONTENT_TYPE_XML;
			}
			_svc.resultFormat=HTTPService.RESULT_FORMAT_TEXT;
		}
		
		protected override function onComplete(e:ResultEvent):void{
			var obj:* = e.result as String;
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
			sendHttpRequest("teacher" , "login" , _ctrller );
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