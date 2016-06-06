package com.onlineclass.net.sock
{
	import flash.utils.ByteArray;

	public class SocketMessage
	{
		/**socket信息header长度*/private static var HEADER_LENGTH:int =60;
		/**gb中文编码*/public  static var GB:String = "cn-gb";
		/**utf8中文编码*/public static var UTF8:String ="utf-8";
		/**header多余的长度补足*/private static var PLACE:String="@";
		/**用户特征码*/private static var SID:String;
		
		/**
		 * 生成socket信息，包含header和body
		 * @param clientype : 发送信息的客户端类型
		 * @param msgtype : 信息类型
		 * @param account : 发送账号
		 * @param msg : 信息正文
		 * @param charset : 字符编码 
		 * */
		public static function buildSoMsg(   clienType:int , msgType:int  ,account:String , msg:String , charset:String="cn-gb"  ):String{
			trace("message to send : "+msg);
			msg = Base64Coder.instance.EncodeUTF8(msg);
			var header : String = buildHead(clienType , msgType , account , msg );
			var msg:String = header.concat( msg );
			return msg;
		}
		
		/**
		 * 生成header
		 * @param clientype : 发送信息的客户端类型 只能是0(student)或1(teacher)
		 * @param msgtype : 信息类型
		 * @param account : 发送账号
		 * @param msg : 信息正文
		 * @param charset : 字符编码 
		 * */
		public static function buildHead(clienType:int , msgType:int  ,account:String , msg:String , charset:String="cn-gb"):String{
			if(clienType!=0 && clienType!=1) throw new Error("Wrong param!");
			var arr:Array = [clienType.toString() , msgType  , account , SID, getStrBytesLength(msg,charset) ];
			var str:String = JSON.stringify( arr );
			while(str.length<HEADER_LENGTH){
				str = str.concat("@");
			}
			return str;
		}
		/**获取一段信息的charset编码下的字节数**/
		public static function getStrBytesLength(str:String , charset:String):int{
			var byte:ByteArray = new ByteArray();
			byte.writeMultiByte( str , charset );
			return byte.length;
		}
		
		/**登陆账户特征码**/
		public static function set sid(value:String):void{
			SID=value;
		}
	}
}