##网络课程APP客户端

* [学生侧](#student)
 * [网络连接](#net)
   * [socket](#socket)
   * [连接状态](#heartbeat)
 * [答题界面](#question)
* [教师侧](#teacher)
 * [数据列表](#list)
 * [柱状图](#chart)

基于视图（view）开发是第一次，基于选项卡那个乍看不太符合这个应用要求，navigator的很多功能都没有太深入研究，所谓“持久化”虽然看过了Adobe官网的教程但这里没有刻意使用。

基本做法就是网络连接收发数据 → 收到数据时发出一个事件，将数据给DataProcessor → DataProcessor将数据转换为相应的对象，侦听到事件的flex获取这个对象并显示。

当然真做起来就没那么简单，也有遇到过不懂的地方，但也学了一段日子的AS3了，虽然手机Flex~~从来~~没有做过几次，这个作品要求不是很复杂，~~到处偷工减料~~做得比较轻松。

此外，电脑在校园网中的IP不固定，应用第一个界面是给调试用来输入IP的，也可以选择Socket、post请求、get请求~~后两个没什么差别就是了~~来通信，
选择socket的情况要求服务器打开socket监听。

##<p id="student">学生侧</p>

学生侧有登陆、选择试卷、作答、看**这一次的成绩**4个功能

看成绩只是在服务器批改完试卷后返回来显示一个数字，做得简陋和作答界面是一个view分开State。

![](https://raw.githubusercontent.com/CloudTsang/OnlineClass/master/picture/test1.png)

###<p id="net">网络连接</p>
在这一部分多做了一些尝试，开始使用socket连接，之后转成http请求后，泛用的urlRequest和专用（？）的HttpService都各写了一遍，也做成了POST和GET可以在打开应用时选择的形式。

网络连接要实现以下接口：
``` as3
public interface IConnection
	{
		/**连接状态，设为true会进行连接，设为false会断开,在http请求的做法中连断都自动了不需要**/
		function set connectStatus(v:Boolean):void;
		/**发送信息*/
		function Send(message:Object):void;
	}
```

在这个应用中，发送的信息只能是以下五种：登陆信息json，试卷请求，试题答案数组，放弃考试和登出学生侧的连接要额外实现这个接口：
``` as3
function login(no:String , pass:String):void;
function testRequest( test:String):void;
function sendAnswer( ans:AnswerInfo ):void;
function giveup(test:String):void;
function logout():void;
```
手机flex没有ApplicationClosed这样的事件~~真是不方便~~，确认是否在线要做心跳检测，放弃考试和登出两个操作相当于让服务器做一次在线情况检查，因为并没有设计出不允许学生放弃考试/登出的情况，结果都会返回true（允许）。

####socket
socket比起未考虑X包问题时增加的编辑header部分，改得不是很多，收发的信息body和http请求是一样的。

做了心跳检测等等一些内容之后，在这里设置了一个SID属性，相当于每个登陆用户的标记。

但其实并不很了解所谓header应该包含什么内容，这里做成一个数组转成的json字符串，数组要有以下元素：
``` as3
         /**
		 * 生成header
		 * @param clientype : 发送信息的客户端类型 只能是0(student)或1(teacher)
		 * @param msgtype : 信息类型
		 * @param account : 发送账号
		 * @param msg : 信息正文
		 * @param charset : 字符编码 
		 * */
		public static function buildHead(clienType:int , msgType:int  ,account:String , msg:String , charset:String="cn-gb"):String{
			if(clienType!=0 && clienType!=1) throw new Error("Wrong param!");
			var arr:Array = [clienType.toString() , msgType  , account , SID, getStrBytesLength(msg,charset) ];
			var str:String = JSON.stringify( arr );
			while(str.length<HEADER_LENGTH){
				str = str.concat("@");
			}
			return str;
		}
```
header给60字节，有多的填充"@"，header.concat(body)这样的字符创发给服务器。

####<p id="heartbeat">连接状态</p>

客户端每隔1段时间访问服务器的某个Action（socket则是发送心跳包什么的）来“证明”自己还在线。

定义了一个新链接来完成这一步，需要实现以下接口：
``` as3
public interface IHeartBeat
	{
		function startHB(min:Number=1):void;
		function stopHB():void
		function get errMsg():String	
	}
```

心跳链接本身的功能是简单，比较麻烦的是接收到下线（？）信息时的处理。

这个应用暂时只做了以下2种异常情况：重复登录，长时间无连接。总之都是要弹出一个警告然后强制登出。

但是Flex手机端没有Alert，只能自己写一个SkinnablePopUpContainer组件。然而组件内的显示对象的坐标之类的却一直调整不好，百度教程也少，这里很笨拙的做了些简单的计算，用AS3写了按钮的部分，并override了组件初始化完成时调用的函数来把button添加上去。
```as3
protected override function initializationComplete():void 
			{
				super.initializationComplete();
				pad.addElement(_btn);
			}


接收到错误信息的场合
(StudentSide.hbcon as EventDispatcher).addEventListener(Basic.COMMU_ERROR , hbAlert);
...
protected function hbAlert(e:Event):void{
				StudentSide.alertWindow.setWindow(systemManager.screen.width , systemManager.screen.height,StudentSide.hbcon.errMsg);
				StudentSide.alertWindow.open(this ,false);
			}

private function alertConfirm(e:Event=null):void{
				if(StudentSide.alertWindow.isOpen) StudentSide.alertWindow.close();
				StudentSide.dispose();
				navigator.popToFirstView();
			}

```
总体来说，虽然在“服务器传来错误信息之后应该干什么呢？”这样的问题上纠结了一阵，但客户端这部分并没有花很多时间。

实现效果如下：

![](https://raw.githubusercontent.com/CloudTsang/OnlineClass/master/picture/test4.png)


###<p id="question">作答界面</p>
比较难的是这个图文混排的题目显示的部分。

Flex的TextArea、RichEditableText都能支持基础的html

...如果是电脑端的话...

~~textFlow只是装饰而已，上面的大人物是不会懂的。~~

~~htmlText是什么，能吃么？~~

可能我的百度奥义熟练度不够高，查不到手机flex自带的组件怎么显示html，之后从flash pro cs6复制过来了tlfruntime.swc，用里面的TLFTextField完成了这一部分。虽然之前也试过使用flash pro的matrixtransform，但这次是找不到其他方法才这样做的~~（· 皿·）~~

``` as3
_tlf.htmlText =_list.selectedItem[LABEL_STEM];
```

TLField要自行Add到舞台上，加入弹出提示的功能后，使用stage.addchild()就会盖住弹出窗口了，这里用的是SpriteVisualElement.
``` as3
visualE.addChild(tlf);
...
<s:SpriteVisualElement id="visualE" height="100%" width="100%" includeIn="RQState"/>
```
也不用在意切换state/view时要使用stage.removeChild()了。

---
总的来说学生侧没有太多花心思的地方。教师侧倒是用到了一些新东西。

##<p id="teacher">教师侧</p>

![](https://raw.githubusercontent.com/CloudTsang/OnlineClass/master/picture/test2.png)

教师侧的网络连接需要实现这个接口
``` as3
public interface ITSender
	{
		/**请求试卷列表**/
		function testListRequest():void;
		/**请求试卷test的classs班级分数**/
		function testRequest(test:String , classs:String):void;
		/**请求有testname试卷成绩的班级列表**/
		function classsRequest(testname:String):void;
		/**请求统计结果**/
		function statRequest(testname:String):void;	
	}
```

###<p id="list">数据列表</p>

教师侧的难点在于显示数据的列表，*如果是电脑的话大概就用DataGrid来做了*，但是手机的list只能靠自定义ItemRenderer来实现。

开始在做列表时会出现“数据无法绑定”的警告，但却能正常工作~~你是怎样喔~~。百度一下有两种方法：
``` as3
//主动配置数据绑定
BindingUtils.bindProperty( labName , "text" , data , "name" );
//这种做法就不用在mxml标签中写text属性
...
//重写数据属性实现绑定
[Bindable] private var _name:String;
[Bindable] private var _data:Object;
public override function set data(value:Object):void{
	_data = value;
    _name = _data.name;
}
...
<s:Label x="0" width="100" text="{_name}" id="labName"/>
```
两种都是可以的，就个人来说由于接触到BindingUtils这种新东西才好所以选的是第一种。

###<p id="chart">柱状图</p>
显示各分数段人数的柱状图，网上图标自定义的教程微妙地有点杂乱，看着官网的API参考尝试了一会儿才完成。

x轴显示“分数范围”range字段
``` as3
_axis = new CategoryAxis();
_axis.categoryField = "range";
_axis.title = "分数段人数统计表";
```

y轴的最低单位为1
``` as3
_linear = new LinearAxis();
_linear.interval=1;
```

综合
``` as3
cs.xField="range";
cs.yField="number";
cs.displayName="人数";
cs.labelField="number";
_series=[cs];

ch.series = _series;
ch.horizontalAxis = _axis;
ch.verticalAxis = _linear;
ch.dataProvider = data;
```


