package com.onlineclass.info
{
	/**记录试题作答的类,socket传输时使用**/
	public class AnswerInfo
	{
		private var _test:String;
		private var _number:String;
		private var _answer:Array;
		/**
		 * @param name ： 答案查询的试卷名称
		 * @param len ： 答案个数
		 * @param no ： 学号
		 * **/
		public function AnswerInfo(name:String , len:int , no:String)
		{
			_test = name;
			_number = no;
			_answer=new Array(len);
			for(var i:int=0 ; i<len ; i++){
				_answer[i]=-1;
			}
		}
		/**设置第no题的答案索引ans*/
		public function setAnswer(no:int , ans:int):void{
			trace("第"+no+"题的选择为："+ans);
			_answer[no] = ans;
		}
		/**获取第no题的答案索引ans*/
		public function getAnswer(no:int):int{
			return _answer[no]
		}
		public function get convertToJson():String{
			var tmp:Object = {
				test:_test,
				answer:_answer,
				number:_number
			}
			return JSON.stringify(tmp);
		}
		/**只将答案数组转换为json并返回*/
		public function get convertToJson_AnsOnly():String{
			return JSON.stringify( _answer );
		}
		
	}
}