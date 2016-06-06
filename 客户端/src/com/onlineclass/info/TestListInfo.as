package com.onlineclass.info
{
	/**试卷集的信息**/
	public class TestListInfo implements IListInfo
	{
		protected var _testlist:Array;
		public function TestListInfo(arr:Array)
		{
			_testlist=new Array();
			for(var i:int=0 ; i<arr.length ; i++){
				_testlist.push( TestInfo.ObjectToInst( arr[i] as Object ) );
			}
		}	
		public function get list():Array{
			return _testlist;
		}
		public static function JsonToInst(js:String):TestListInfo{
			var obj:Object = JSON.parse(js) as Object;
			return ObjectToInst(obj);
		}
		public static function ObjectToInst(obj:Object):TestListInfo{
			var arr:Array = new Array();
			for(var key:String in obj){
				var tmp : Object = { 
					_id:key,
					description:obj[key]
				};
				arr.push(tmp);
			}
			return new TestListInfo(arr);
		}
	}
}