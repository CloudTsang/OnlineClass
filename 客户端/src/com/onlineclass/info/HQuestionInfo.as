package com.onlineclass.info
{
	/**1道题目的信息**/
	public class HQuestionInfo
	{
		protected var _question:String;
		protected var _no:int;
		/**
		 * @param q : 题干
		 * @param no : 题号
		 * */
		public function HQuestionInfo(q:String , no:int)
		{
			_question = q;
			_no = no;
		}
		
		/**题号**/
		public function get number():int{
			return _no
		}
		/**题干**/
		public function get question():String{
			return _question;
		}
		public static function JsonToInst(str:String , no:int):HQuestionInfo{
			var tmp:Object = JSON.parse(str);
			return ObjectToInst(tmp , no);
		}
		public static function ObjectToInst(obj:Object , no:int):HQuestionInfo{
			var str:String= Base64Coder.instance.Decode( obj as String );
			return new HQuestionInfo(str  , no);
		}
	}
}