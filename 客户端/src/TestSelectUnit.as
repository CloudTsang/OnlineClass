package
{
	import com.onlineclass.info.TestInfo;
	import com.onlineclass.info.TestListInfo;
	import com.onlineclass.net.ISender;
	import flash.events.MouseEvent;
	import spark.components.Button;
	import spark.components.Label;
	import spark.components.List;
	/**试卷选择单元*/
	public class TestSelectUnit extends ListUnitTemplate
	{
		private const LABEL_NAME:String="name";
		private const LABEL_DES:String="description";
		private var _txtName:Label;
		private var _txtDes:Label;
		private var _btnConf:Button;
		private var _sender:ISender;
		/**
		 * @param tl ：试卷列表信息　　@param l ：显示列表
		 * @param txtN ：试卷名文本      @param txtD ： 试卷描述文本
		 * @param btn ：确认键         @param com ： 连接
		 * **/
		public function TestSelectUnit(tl:TestListInfo, l:List , txtN:Label , txtD:Label , btn:Button , con:ISender)
		{
			super(tl , l);
			_list.labelField = LABEL_NAME;
			_list.selectedIndex=0;
			_txtName = txtN;
			_txtDes  = txtD;
			_btnConf = btn;
			_sender  =con;
			renewLabel();
			_btnConf.addEventListener(MouseEvent.CLICK , confirmHandler);
		}
		protected override function listClickHandler(e:MouseEvent):void{
			renewLabel();
		}
		private function confirmHandler(e:MouseEvent):void{
			try{
				_sender.testRequest( _list.selectedItem[LABEL_NAME] );
			}catch(err:Error){
				trace(err.getStackTrace());
			}
		}
		protected override function renewLabel():void{
			_txtName.text = _list.selectedItem[ LABEL_NAME ];
			_txtDes.text = (_list.selectedItem as TestInfo).description;
		}
	}
}