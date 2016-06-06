package com.onlineclass.teacherside
{
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.events.MouseEvent;
	import flash.ui.Mouse;
	import mx.collections.ArrayCollection;
	import spark.components.Button;
	import spark.components.Label;
	import spark.components.List;
	import views.teacher.TScoreView;
	import views.teacher.TStatView;
	import com.onlineclass.teacherside.net.ITSender;
	/**教师侧试卷-班级列表单元。包含2个列表对象*/
	public class TestNClassListUnit extends EventDispatcher
	{
		protected var _tList:List;
		protected var _tArrc:ArrayCollection;
		protected var _tArr:Array;
		protected var _cList:List;
		protected var _cArrc:ArrayCollection;
		protected var _cArr:Array;
		
		protected var _lab:Label;
		protected var _btnStat:Button;
		protected var _btnConf:Button;
		protected var _sender:ITSender;
		protected var _cache:Array;
		protected var _haveCache:Array;
		/**
		 * @param list1 ： 显示试卷名的列表     @param list2 ： 显示班级的列表
		 * @param test ： 试卷信息   @param lab ： 提示文本
		 * @param sender ： 教师侧网络连接  @param btn1 ： 统计按钮  @param btn2 ： 查看分数按钮
		 * */
		public function TestNClassListUnit(list1:List , list2:List ,test:Array , lab:Label , sender:ITSender ,btn1:Button , btn2:Button)
		{
			_tList = list1;
			_cList = list2;
			_sender = sender;
			_lab =lab;
			_btnStat = btn1;
			_btnConf = btn2;
			
			_tArrc = new ArrayCollection();
			_tArr = test;
			_tArrc.source = _tArr;
			_tList.dataProvider = _tArrc;
			
			_cArrc=new ArrayCollection();
			
			_haveCache=new Array();
			_cache = new Array();
			
			_tList.addEventListener(MouseEvent.CLICK , Statable);			
			_tList.addEventListener(MouseEvent.CLICK , testList_clickHandler);
		}
		/**清除预存的列表&数据*/
		public function cleanCache():void{
			_haveCache=new Array();
			_cache = new Array();
			_tList.selectedIndex = -1;
			_cList.selectedIndex = -1;
			TStatView.statName = "";
			_sender.testListRequest();
		}
		
		protected function testList_clickHandler(e:MouseEvent):void{
			for(var i:int=0 ; i<_haveCache.length ; i++){
				if( _haveCache[i] == _tList.selectedItem ) {
					setClasss( _cache[i] );
					trace("该班级列表已缓存");
					return;
				}
			}
			_sender.classsRequest( _tList.selectedItem as String );
			clickAble(false,false);
		}
		/**更新班级列表*/
		public function setClasss(classs:Array):void{
			_haveCache.push(_tList.selectedItem as String);
			_cache.push(classs);
			_cArr = classs;
			_cArrc.source=_cArr;
			_cList.dataProvider = _cArrc;
		}
		
		protected function Statable(e:MouseEvent):void{
			_btnStat.visible = true;
			_btnStat.addEventListener(MouseEvent.CLICK , statHandler);
			_cList.addEventListener( MouseEvent.CLICK , confirmAble);
		}
		
		protected function statHandler(e:MouseEvent):void{
			if(TStatView.statName == _tList.selectedItem){
				this.dispatchEvent(new Event("goStat"));//该试卷的统计结果已缓存
				return ;
			}
			_sender.statRequest( _tList.selectedItem as String );
			TStatView.statName = _tList.selectedItem as String;
		}
		
		protected function confirmAble(e:MouseEvent):void{
			_btnConf.visible=true;
			_btnConf.addEventListener(MouseEvent.CLICK,confirmHandler);
		}
		protected function confirmHandler(e:MouseEvent):void{
			if(TScoreView.test == _tList.selectedItem && TScoreView.classs ==_cList.selectedItem as String){
				this.dispatchEvent(new Event("goScore"));//该试卷的分数数据已缓存
				return;
			}
			_sender.testRequest( _tList.selectedItem as String , _cList.selectedItem as String);
			TScoreView.test = _tList.selectedItem as String;
			TScoreView.classs =_cList.selectedItem as String;
			_lab.text="正在请求分数数据，请稍候...";
		}
		
		public function clickAble(test:Boolean , classs:Boolean):void{
			_tList.mouseEnabled = test;
			_tList.mouseEnabled = classs;
		}
	}
}