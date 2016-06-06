package
{
	import com.onlineclass.info.IListInfo;
	import flash.events.MouseEvent;
	import mx.collections.ArrayCollection;
	import spark.components.List;

	public class ListUnitTemplate
	{
		protected var _arrc:ArrayCollection;
		protected var _info:IListInfo;
		/**列表显示的对象信息*/protected var _list:List;
		public function ListUnitTemplate(info:IListInfo , list:List)
		{
			_info=info;
			_list=list;
			_arrc=new ArrayCollection;
			_arrc.source=_info.list;
			_list.dataProvider=_arrc;
			_list.addEventListener( MouseEvent.CLICK , listClickHandler);
		}
		protected function listClickHandler(e:MouseEvent):void{
			throw new Error("This function should be overrided!");
		}
		protected function renewLabel():void{
			throw new Error("This function should be overrided!");
		}
	}
}