package com.onlineclass.info
{
	/**试题列表信息**/
	public class HQuestionListInfo implements IListInfo
	{
		private var _qlist:Array;
		private var _time:int;
		private var _name:String;
		/**
		 * @param n ： 试卷名 
		 * @param t ： 限制时间
		 * @param q ： 题干数组
		 * */
		public function HQuestionListInfo(n:String , t:int, q:Array)
		{
			_name = n;
			_time = t;
			_qlist = new Array();
			
			for(var i:int=0 ; i<q.length ; i++){
				_qlist.push( HQuestionInfo.ObjectToInst( q[i] as String  , i+1) );
			}
		}
		
		/**题目信息的数组**/
		public function get list():Array{
			return _qlist;
		}
		/**试卷时间限制**/
		public function get timelimit():int{
			return _time;
		}
		/**试卷名称**/
		public function get testName():String{
			return _name;
		}
		/**题目数量*/
		public function get qNumber():int{
			return _qlist.length;
		}
		public static function JsonToInst(str:String):HQuestionListInfo{
			var obj:Object = JSON.parse(str);
			return new HQuestionListInfo( obj.name , obj.time , obj.paper )
		}
		
		public static function ObjectToInst(obj:Object):HQuestionListInfo{
			return new HQuestionListInfo( obj.name , obj.time , obj.paper )
		}
	}
}