package com.onlineclass.info
{
	/**登陆信息**/
	public class LoginInfo
	{
		/**生成登陆信息json字符串
		 * @param no：学号
		 * @param pass：密码 
		 * **/
		public static function convertToJson(no:String , pass:String):String{
			var tmp:Object = {
				number:no,
				password:pass
			}
			return JSON.stringify( tmp );
		}
	}
}