package com.onlineclass.net.mvc
{
	import com.onlineclass.info.AnswerInfo;
	import com.onlineclass.info.LoginInfo;
	import com.onlineclass.net.IConnection;
	import com.onlineclass.net.ISender;
	
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.net.URLRequestMethod;
	import flash.net.URLVariables;
	
	import mx.rpc.events.ResultEvent;
	import mx.rpc.http.HTTPService;
	
	import spark.components.Label;
	import spark.core.ContentRequest;
	
	public class TestolConnectionHSVC  extends HSVConnection implements ISender
	{
		private var _lab:Label;
		private var _ctrller:String;
		private var _type:int;
		public function TestolConnectionHSVC(lab:Label=null  ,method:String="POST" )
		{
			super(method);
			if(lab) _lab= lab;
			if(method =="GET") {
				_isGET=true;
				_ctrller = "Student";
			}
			else {
				_isGET=false;
				_ctrller = "PStudent";
				_svc.contentType=HTTPService.CONTENT_TYPE_XML;  //service.contentType = "application/json";
			}
			
			_svc.resultFormat=HTTPService.RESULT_FORMAT_TEXT;
		}
		protected override function onComplete(e:ResultEvent):void{
			var obj:* = e.result as String;
			trace(obj);
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

		public function login(no:String, pass:String):void
		{
			if(_lab)_lab.text="正在验证登陆信息，请稍候...";
			var obj:Object = LoginInfo.convertToJson(no,pass);
			if(_isGET){
				var str:String =Base64Coder.instance.EncodeUTF8(obj as String);
				obj={ info:str };
			}
			_type = Basic.DATA_LOGINSUCCESS;
			sendHttpRequest( obj , "login" , _ctrller );
		}
		
		public function testRequest(test:String):void
		{
			if(_lab)_lab.text="正在向服务器请求试题，请稍候...";
			var obj:Object;
			if(_isGET) obj = {info:Base64Coder.instance.EncodeUTF8( test )};
			else obj = test;
			sendHttpRequest( obj , "testRequest",_ctrller);
			_type = Basic.DATA_QUESTIONS;
		}
		
		public function sendAnswer(ans:AnswerInfo):void
		{
			if(_lab)_lab.text="正在向服务器发送答案，请等待批改结果...";
			var str:String = ans.convertToJson_AnsOnly;
			var obj:Object;
			if(_isGET) obj ={info:Base64Coder.instance.EncodeUTF8(str)};
			else obj = str;
			sendHttpRequest( obj , "answerCheck" , _ctrller );
			_type = Basic.DATA_SCORE;
		}
		public function giveup(test:String):void{
			if(_lab)_lab.text="正在请求中止本次测验，请稍后...";
			var obj:Object;
			if(_isGET) obj={info:Base64Coder.instance.EncodeUTF8(test)};
			else obj=test;
			sendHttpRequest(obj ,"giveup",_ctrller);
			_type = Basic.DATA_GIVEUP;
		}
		public function logout():void
		{
			var obj:Object;
			if(_isGET) obj ={info:"exit"}
			else obj="exit"
			sendHttpRequest(obj  , "logout" , _ctrller);
			_type = Basic.DATA_LOGOUT;
		}
	}
}