package com.onlineclass.teacherside.chartrender
{
	import mx.charts.CategoryAxis;
	import mx.charts.ColumnChart;
	import mx.charts.LinearAxis;
	import mx.charts.series.ColumnSeries;
	import mx.collections.ArrayCollection;
	import spark.effects.easing.Linear;
	/**分数段人数统计柱状图设定*/
	public class ChartCreator
	{
		private var _series : Array;
		private var _axis : CategoryAxis;
		private var _linear:LinearAxis;
		public function ChartCreator(ch:ColumnChart , data:ArrayCollection)
		{
			var cs:ColumnSeries = new ColumnSeries();
			cs.xField="range";
			cs.yField="number";
			cs.displayName="人数";
			cs.labelField="number";
			_series=[cs];
			
			_axis = new CategoryAxis();
			_axis.categoryField = "range";
			_axis.title = "分数段人数统计表";
			
			_linear = new LinearAxis();
			_linear.interval=1;
			
			ch.series = _series;
			ch.horizontalAxis = _axis;
			ch.verticalAxis = _linear;
			ch.dataProvider = data;
		}
	}
}