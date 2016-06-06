package com.onlineclass.teacherside.info
{
	import mx.states.State;
	/**1条统计结果信息*/
	public class StatInfo
	{
		/**班级**/public var classs:String;
		/**平均分**/public var average:Number;
		/**方差**/public var variance:Number;
		/**分数段人数统计**/public var number:Array;
		/**
		 * @param cl ： 班级
		 * @param avg ： 平均分
		 * @param vrc ： 方差
		 * @param num ： 分数段人数数组
		 * */
		public function StatInfo(cl:String ,avg:Number , vrc:Number , num:Array)
		{
			classs = cl;
			average = avg;
			variance = vrc;
			number = num;
		}
		
	
		public static function JsonToInst(js:String):StatInfo{
			var obj:Object = JSON.parse(js);
			return new StatInfo(obj.classs , obj.average , obj.variance , obj.number);
		}
		public static function ObjToInst(obj:Object):StatInfo{
			return new StatInfo(obj.classs , obj.average , obj.variance , obj.number);
		}
	}
}