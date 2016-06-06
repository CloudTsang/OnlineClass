package
{
	import com.onlineclass.info.AnswerInfo;
	import com.onlineclass.info.IListInfo;
	import com.onlineclass.info.StudentInfo;
	import com.onlineclass.info.HQuestionInfo;
	import com.onlineclass.info.HQuestionListInfo;
	import com.onlineclass.net.ISender;
	import fl.text.TLFTextField;
	import flash.display.DisplayObject;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.events.TimerEvent;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.utils.Timer;
	import flashx.textLayout.conversion.TextConverter;
	import flashx.textLayout.elements.TextFlow;
	import mx.events.ItemClickEvent;
	import spark.components.Button;
	import spark.components.Label;
	import spark.components.List;
	import spark.components.RadioButtonGroup;
	import spark.components.TextArea;
	import spark.components.supportClasses.StyleableTextField;
	/**试卷回答单元*/
	public class HQuestionUnit extends ListUnitTemplate
	{
		private const LABEL_NO:String = "number";
		private const LABEL_STEM:String = "question";
		private const SEL_NUM:int=4;
		
		private var _txtTime:Label;
		private var _tlf:TLFTextField;
		private var _txtQNo:Label;
		
		private var _btns:RadioButtonGroup;
		private var _btnComp:Button;
		private var _btnPrev:Button;
		private var _btnNext:Button;
		private var _btnGiveUp:Button;
		
		private var _sender:ISender;
		private var _compQ:Vector.<Boolean>;
		private var _timer:Timer;
		private var _time:int;
		private var _qnum:int;
		private var _answered:int;
		protected var _ans:AnswerInfo;
		
		
		/**@param ql:试题列表信息  @param l:列表对象 
		 * @param txtT:时限文本  @param txtQ:题干文本框
		 * @param txtQNo:题目数文本 @param txtN:试卷名文本
		 * @param vecBtn:单选按钮组
		 * @param btnComp:提交按钮    @param btnP:前一题按钮   @param btnN:下一题按钮
		 * @param con:服务器连接
		 * */
		public function HQuestionUnit(ql:HQuestionListInfo , l:List , txtT:Label   , txtQNo:Label, txtN:Label,
									  vecBtn:RadioButtonGroup , 
									  btnComp:Button, btnGive:Button,btnP:Button,btnN:Button , 
									  con:ISender , id:StudentInfo)
		{
			super(ql, l);
			
			_list.labelField=LABEL_NO;
			_txtTime=txtT;
			_txtQNo=txtQNo;
			txtN.text = ql.testName;
			_btns=vecBtn;
			_btnNext=btnN;
			_btnPrev=btnP;
			_btnComp=btnComp;
			_btnGiveUp=btnGive;
			_sender=con;
			_ans = new AnswerInfo(ql.testName , ql.qNumber , id.number);
			_timer=new Timer(1000);
			_time = ql.timelimit*60;
			_txtTime.text=String(_time);
			_qnum=_info.list.length;
			_answered=0;
			_txtQNo.text=String(_answered)+"/"+String(_qnum);
			_list.selectedIndex=0;
			_tlf = createTLF();
			_compQ=new Vector.<Boolean>;
			for(var i:int=0 ; i<_qnum ; i++){
				_compQ[i]=false;
			}
			renewLabel();
			_timer.addEventListener( TimerEvent.TIMER , timerHandler);
			_btnComp.addEventListener( MouseEvent.CLICK ,Submit);
			_btnGiveUp.addEventListener(MouseEvent.CLICK , Giveup);
			_btns.addEventListener(Event.CHANGE , select);
			_btnPrev.addEventListener(MouseEvent.CLICK,btnSelect);
			_btnNext.addEventListener(MouseEvent.CLICK,btnSelect);
			_timer.start();
		}
		
		/**提交答案*/
		protected function Submit(e:MouseEvent=null):void{
			_sender.sendAnswer( _ans );
			TestOver();
		}
		/**放弃考试*/
		protected function Giveup(e:MouseEvent=null):void{
			_sender.giveup( (_info as HQuestionListInfo).testName );
		}
		/**考试结束*/
		public function TestOver(e:Event=null):void{
			if(_timer.running)_timer.stop();
			_list.removeEventListener(MouseEvent.CLICK , listClickHandler);
			_btns.removeEventListener(ItemClickEvent.ITEM_CLICK , select);
		}
		
		protected override function  listClickHandler(e:MouseEvent):void{
			renewLabel();
		}
		/**切换题目时更新文本*/
		protected override function renewLabel():void{
			_tlf.htmlText ="<p/>"+ _list.selectedItem[LABEL_STEM];
			//做过的题目显示已选的选项
			switch(_ans.getAnswer( _list.selectedIndex )){
				case -1:
					_btns.selectedValue = null
					break;
				case 0:
					_btns.selectedValue = "A"
					break;
				case 1:
					_btns.selectedValue = "B"
					break;
				case 2:
					_btns.selectedValue = "C"
					break;
				case 3:
					_btns.selectedValue = "D"
					break;
			}
		}
		/**上/下一题按钮处理*/
		private function btnSelect(e:MouseEvent):void{
			switch(e.target){
				case _btnPrev:
					if(_list.selectedIndex>0)_list.selectedIndex--;
					break;
				case _btnNext:
					if(_list.selectedIndex<_qnum-1)_list.selectedIndex++;
					break;
			}
			renewLabel();
		}
		/**单选按钮处理*/
		private function select(e:Event):void{
			var tmp:int;
			switch(_btns.selection.label)
			{
				case "A":
					tmp = 0;
					break;
				case "B":
					tmp = 1;
					break;
				case "C":
					tmp = 2;
					break;
				case "D":
					tmp = 3;
					break;
			}
			_ans.setAnswer( _list.selectedIndex , tmp );
			if( !_compQ[_list.selectedIndex] ){
				_compQ[_list.selectedIndex]=true;
				_answered++;
				_txtQNo.text=String(_answered)+"/"+String(_qnum);
			}
		}
		/**题目显示文本*/
		public function get questionText():DisplayObject{
			return _tlf;
		}
		private function createTLF():TLFTextField{
			var tlf:TLFTextField = new TLFTextField();
			tlf.wordWrap=true;
			tlf.multiline=true;
			var format:TextFormat=new TextFormat();
			format.size=18;
			tlf.defaultTextFormat=format;
			return tlf;
		}
		
		private function timerHandler(e:TimerEvent):void{
			_time--;
			_txtTime.text=String(_time);
			if(_time==0){
				_timer.stop();
			}
		}
	}
}