package com.onlineclass.teacherside.info
{
	import com.onlineclass.teacherside.ChartObject;
	/**统计列表信息*/
	public class StatListInfo
	{
		protected var _sarr:Array;
		protected var _carr:Array; 
		/**
		 * @param arr ： 这个数组元素应当全部为StatInfo，即Vector.<StatInfo>这样的对象
		 * */
		public function StatListInfo(arr:Array)
		{
			_sarr = new Array();
			_carr = new Array();
			//将分数统计和分数段人数统计分开
			for each(var i:Object in arr){
				var tmp:StatInfo = StatInfo.ObjToInst( i);			
				_sarr.push( { classs:tmp.classs , average:tmp.average , variance:tmp.variance } );
				var tmpa:Array = tmp.number;
				var tmpCo:Array = [
					new ChartObject("100~90" , tmpa[0]),
					new ChartObject("89~80" , tmpa[1]),
					new ChartObject("79~70" , tmpa[2]),
					new ChartObject("69~60" , tmpa[3]),
					new ChartObject("59~" , tmpa[4]),
					];
				_carr.push(tmpCo);
			}
		}
		/**分数统计数组**/
		public function get scoreList():Array{
			return _sarr;
		}
		/**分数段人数统计数组**/
		public function get chartList():Array{
			return _carr;
		}
		
		public static function JsonToInst(js:String):StatListInfo{
			var arr:Array = JSON.parse(js) as Array;
			return new StatListInfo(arr);
		}
		public static function ObjectToInst(arr:Array):StatListInfo{
			var tmp:Array = new Array();
			for each(var i:String in arr){
				tmp.push(JSON.parse(i));
			}
			return new StatListInfo(tmp);
		}
	}
}