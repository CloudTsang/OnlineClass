package com.onlineclass.net.mvc
{
	import com.onlineclass.info.AnswerInfo;
	import com.onlineclass.info.LoginInfo;
	import com.onlineclass.net.ISender;
	
	import flash.events.Event;
	import flash.net.URLRequestHeader;
	import flash.net.URLVariables;
	
	import mx.rpc.http.HTTPService;
	import mx.utils.Base64Decoder;
	import mx.utils.Base64Encoder;
	
	import spark.components.Label;
	
	public class TestolConnectionMVC extends MVConnection implements ISender
	{
		protected var _lab:Label;
		private var _type:int;
		private var _ctrller:String;
		
		public function TestolConnectionMVC(lab:Label=null , type:String = "POST")
		{
			super(type);
			if(lab)_lab=lab;
			if(_isGET) _ctrller = "Student";
			else _ctrller = "PStudent";
		}
		
		protected override function onComplete(e:Event):void{
			var obj:* = e.target.data.toString();
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
			var str:String = LoginInfo.convertToJson(no,pass);
			if(_isGET) str ="info=" + Base64Coder.instance.EncodeUTF8(str);
			sendHttpRequest( str , "login" , _ctrller );
			_type = Basic.DATA_LOGINSUCCESS;
		}
		
		public function testRequest(test:String):void
		{
			if(_lab)_lab.text="正在向服务器请求试题，请稍候...";
			if(_isGET) test = "info=" +Base64Coder.instance.EncodeUTF8( test );
			sendHttpRequest( test , "testRequest" , _ctrller);
			_type = Basic.DATA_QUESTIONS;
		}
		
		public function sendAnswer(ans:AnswerInfo):void
		{
			if(_lab)_lab.text="正在向服务器发送答案，请等待批改结果...";
			var str:String = ans.convertToJson_AnsOnly;
			trace("发送答案："+str);
			if(_isGET) str ="info="+ Base64Coder.instance.EncodeUTF8(str);
			sendHttpRequest( str , "answerCheck" , _ctrller );
			_type = Basic.DATA_SCORE;
		}
		public function giveup(test:String):void{
			if(_lab)_lab.text="正在请求中止本次测验，请稍后...";
			if(_isGET) test="info="+Base64Coder.instance.EncodeUTF8(test);
			sendHttpRequest(test ,  "giveup" ,_ctrller);
			_type = Basic.DATA_GIVEUP;
		}
		public function logout():void{
			sendHttpRequest( "exit" , "logout" , _ctrller); 
			_type = Basic.DATA_LOGOUT;
		}
	}
}