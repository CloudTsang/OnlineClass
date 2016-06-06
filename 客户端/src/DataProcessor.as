package
{
	import com.onlineclass.info.HQuestionListInfo;
	import com.onlineclass.info.IListInfo;
	import com.onlineclass.info.StudentInfo;
	import com.onlineclass.info.TestInfo;
	import com.onlineclass.info.TestListInfo;
	import com.onlineclass.teacherside.info.StatListInfo;
	
	import mx.collections.ArrayCollection;
	
	/**数据处理类**/
	public class DataProcessor
	{
		/**从服务器获得的数据都会储存在这里*/
		private var _data:Array;
		/**从服务器获得的数据类型都会储存在这里*/
		private var _type:Vector.<int>;
		private  static var _ins:DataProcessor; 
		
		public function DataProcessor(){
			if(_ins!=null)throw new Error("This is a singleton!");
			_data=new Array;
			_type=new Vector.<int>;
		}
		
		/**初始化单例类，清空服务器传来的数据**/
		public static function Init():void{
			_ins=null;
			_ins=new DataProcessor();
		}
		
		public static function get instance():DataProcessor{
			return _ins;
		}
		
		/**服务器最新传来的数据的类型**/
		public function get dataType():int{
			return _type[_type.length-1];
		}
		protected function set dataType(t:String):void{
			_type.push( t);
		}
		
		/**服务器最新传来的数据**/
		public function get data():*{
			return _data[ _data.length-1 ];
		}
		protected function set data(value:*):void{
			_data.push(value);
		}
		
		/**将解析json获得的obj转换为type类型的数据**/
		public function convertToInst(obj:* , type:int):*
		{
			var tmp:*;
			switch(type)
			{
				//学生端登陆成功：包含学生信息和试卷列表
				case Basic.DATA_LOGINSUCCESS:
					tmp = [ StudentInfo.JsonToInst( obj[0] )	, TestListInfo.JsonToInst( obj[1] )  , Number(obj[2]) , obj[3] as String];
					break;
				//学生端试卷题目
				case Basic.DATA_QUESTIONS:
					tmp =HQuestionListInfo.ObjectToInst(obj);
					break;
				case Basic.DATA_GIVEUP:
					tmp = Boolean(obj);
					break;
				//学生端分数，就只是个数字
				case Basic.DATA_SCORE:
					tmp = Number(obj);
					break;
				case Basic.DATA_HBEAT:
					tmp = obj as String;
					break;
				case Basic.DATA_LOGOUT:
					return;
				
				//教师端登陆成功：试卷列表
				case Basic.DATA_TEACHER:
					tmp = obj as Array;
					break;
				//教师端班级列表
				case Basic.DATA_TCLASS:
					tmp = obj as Array;
					break;
				//教师端分数
				case Basic.DATA_TSOCRE:
					tmp = new ArrayCollection();
					tmp.source  =obj;
					break;
				//教师端统计数据
				case Basic.DATA_STAT:
					tmp = StatListInfo.ObjectToInst(obj);
					break;
				//错误信息
				case Basic.DATA_ERR:
					tmp= obj as String;
					break;
			}
			_data.push(tmp);
			_type.push(type);
			return tmp;
		}
	}
}