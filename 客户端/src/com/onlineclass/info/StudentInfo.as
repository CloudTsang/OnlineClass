package com.onlineclass.info
{
	import flash.events.EventDispatcher;
	import flash.events.IEventDispatcher;

	/**学生信息，在登陆成功后收到服务器返回的详细信息**/
	public class StudentInfo extends EventDispatcher
	{
		protected var _name:String;
		protected var _number:String;
		protected var _classs:String;
		/**
		 * @param n ： 姓名
		 * @param no ： 学号
		 * @param c ： 班级
		 * */
		public function StudentInfo(n:String , no:String , c:String)
		{
			_name=n;
			_number=no;
			_classs=c;
		}
		/**姓名**/
		public final function get name():String{
			return _name;
		}
		/**班级**/
		public final function get classs():String{
			return _classs;
		}
		/**学号**/
		public final function get number():String{
			return _number;
		}		
		public final function get numberByINT():int{
			return int(_number);
		}
		
		public static  function JsonToInst(js:String):StudentInfo{
			var tmp:Object = JSON.parse(js);
			return new StudentInfo(tmp.name , tmp.number , tmp.classs);
		}
		public static function ObjectToInst(obj:Object):StudentInfo{
			return new StudentInfo(obj.name , obj.number , obj.classs);
		}
		
	}
}