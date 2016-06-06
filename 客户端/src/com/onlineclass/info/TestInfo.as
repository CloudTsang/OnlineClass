package com.onlineclass.info
{
	/**1份试卷的信息**/
	public class TestInfo
	{
		private var _name:String;
		private var _description:String;
		/**
		 * @param n ： 试卷名
		 * @param d ： 试卷描述
		 * */
		public function TestInfo(n:String , d:String)
		{
			_name = n;
			_description = d;
		}
		/**试卷的名称**/
		public function get name():String{
			return _name;
		}
		/**试卷的描述**/
		public function get description():String{
			return _description;
		}
		
		public static function ObjectToInst(obj:Object):TestInfo{
			return new TestInfo(obj._id , obj.description);
		}
		public static function JsonToInst(json:String):TestInfo{
			var obj:Object = JSON.parse(json);
			return ObjectToInst(obj);
		}
	}
}