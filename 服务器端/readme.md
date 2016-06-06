##网络课程APP服务器端

* [简介](#promo)
 * [获取POST请求数据](#post) 
 * [数据库连接](#database1)
 * [登陆信息](#login)
* [学生侧](#student)
 * [controller](#controller)
   *  [AOP](#AOP)
 * [请求处理](#request)
 * [数据库连接](#database2)
 * [cookies](#cookies)
* [教师侧](#teacher)
* [socket](#socket)
* [心跳检测](#heartbeat)

从0学起用C#写的服务器，刚开始使用Socket，之后改成了用MVC。

C#和AS3比就是格式不太一样，学起来没有很费心思，当然我还有很多没有触及的地方，因为开始只是为了做毕设学的C#，用不到的功能就没太管。

通过nuget、各种引用扩展功能的方便与自由可谓远超AS3的强大，这个作品只需从nuget安装以下程序包
* Newtonsoft.Json
* csredis //完全无料没有读写条数限制的redis驱动，功能是很简单~~就是redis命令行~~但也够用。~~懒得找旧版本的servicestack.redis~~
* （Legacy） C# Driver for MongoDB //提供旧版的MongoDB操作方法。

此外多线程是AS3没有的，我还不太会操作这个，什么时候要lock什么变量之类，做Socket开侦听时有接触，MVC中则没有使用。

---
##<p id="promo">简介</p>

主要工作流程是用户访问控制器的动作，服务器将信息传给事件处理器EvtHandler，EvtHandler处理之后设置一个返回信息sendBack，控制器return这个字符串。


``` c#
控制器：
public virtual String login(String info=null)
		{
            _hdl.loginHandler(info);
			return _hdl.sendBack;
		}

事件处理器：
protected String _back;
public IInfo loginHandler(String info){
    ...
    _back = Basic.convertToBase64( str );
}
public String sendBack{
			get{
				return _back;
			}
		}
```

###<p id="post">获取POST请求数据</p>
这里没有烦直接百度了一段
``` c#
               using(Stream s = HttpContext.Current.Request.InputStream){
					int pos = 0;
					byte[] buffer = new byte[1024];
					StringBuilder builder = new StringBuilder();
					while ( (pos = s.Read(buffer , 0 , 1024)) >0) {
						builder.Append( Encoding.UTF8.GetString(buffer,0,pos) );
					}
					return builder.ToString();
				}
```
实际上作品中用的代码是分开byte[]、string、<*T*>三种返回类型，更加方便了。

###<p id="database1">数据库连接</p>
无论处理来自学生还是教师的请求，都需要向数据库请求的数据是学生信息和试卷列表。
``` c#
    interface IDbConBase
    {
        /**获取1个学号为no的学生资料*/
        StudentInfo getStudent(String no);
        /**获取试卷列表*/
        String getTestList();
    }
```
试卷列表放在mongodb中，虽然不像redis时只是一条json拿出来就直接发客户端，但是驱动提供了相应方法简化这些操作：
``` c#
public virtual String getTestList()
        {
//          return rcon.Get("TestList");
//使用redis的情况就只是这么1句
            var col = mcon.GetCollection("TestList");
            var list = col.FindAll().ToJson();
            return list;
```
还需要创建试卷名和数据库key的哈希表，这个表是静态的，在不为null时传入true的参数才会重新创建一个表
``` c#
public static void createTestHT(RedisClient rcon , Boolean isnew=false)
        {
            if (isnew == false && testHT != null) return;
            testHT = new Hashtable();
            foreach (var element in rcon.HGetAll("TestName"))
            {
                testHT.Add(element.Key , element.Value);
            }
        }
```

###<p id="login">登陆信息</p>
用户身份信息有三种：客户端发来的账号密码、教师信息、学生信息，写起来刚好是后一个继承前一个，但教师侧并没有做登陆验证，因此教师信息没有使用，也没有加进数据库。
``` c#
public class LoginInfo : IInfo
	{
	    [JsonIgnore] public ObjectId _id;
		public String number;
		public String password;
        public String convertToJson(){
			return JsonConvert.SerializeObject(this);
            // IInfo接口要实现返回Json字符串的方法。
		}
    }

public class TeacherInfo :LoginInfo
    {
		public String name;//教师增加了姓名属性
	}

public class StudentInfo : TeacherInfo
	{
		public String classs;//学生增加了班级属性
        [JsonIgnore]
		public StudentInfo ClientSide{
       //学生信息还要发给客户端，在这里生成一个没有密码的信息再发回去。
			get{
			    return new StudentInfo(number , name ,classs , "********");
			}
		}
    }


```


##<p id="student">学生侧</p>


####controller
学生侧的控制器要有以下Action：
``` c#
public virtual String login(String info=null);
//客户端可以用GET或POST测试，所以控制器里写了默认为null的参数。
public virtual  String testRequest(String info=null);
public  virtual String answerCheck(String info=null);
```

####<p id="AOP">AOP</p>
在这期间学到了“面向切口编程”的知识，由于时间关系~~实际上也没有很懂~~客户端并没有改动可以使用Promise的部分，但是服务器端就使用了MVC的Filter。

但其实也只写了两个filter：用户网络状态验证的filter，只是将原来写在Action中的1行使用cookie操作类的代码c+xv过去。另一个则是将信息编解码的filter，这个做分别了对应GET和POST两种请求，attribute直接加到controller上边，顺便将原来的编解码器分开成指令的形式，实现以下接口：
``` c#
    public interface IDeCode
    {
        String decode(String str);
    }
    public interface IEnCode
    {
        String encode(String str);
    }

    public TolCoder(IEnCode en=null, IDeCode de=null)
        {
            var tmp = new DENCode_Empty();
            if (en != null) _enCmd = en;
            else _enCmd = tmp;
            if (de != null) _deCmd = de;
            else _deCmd = tmp;
        }
```
之后写了一个枚举和静态方法来生成指令，试图做到方便地增加编解码的方式与组合。

####<p id="request">请求处理</p>
学生侧需要处理的请求有3个：登录验证、发送试卷题目、批改并返回成绩。
``` c#
public interface IStudentEvt
	{
		/**登陆处理，登陆失败时返回null，成功时返回该学生的信息，sendBack设为学生信息和试卷列表的json*/
		IInfo loginHandler(String info);
		/**获取试卷题目，成功时返回试卷的"试卷名&&限制时间"这样的字符串*/
		String getQuestion(String testname);
		/**批改试卷分数，返回分数*/
		int getScore(String ans,String num, String testname);
		/**试卷0分**/
		void Score0(String testname , String num);
```
调用数据库连接以及一些cookies的操作都在这个类中。

登陆成功后发回去的是学生信息和试卷列表，~~好像~~是这个应用中唯一一个要同时发两种数据的情况，并不清楚正确的做法是什么，这里只是定义了一个数组整个转Json发过去。
``` c#
string[] arr = {ser.ClientSide.convertToJson() , _dbcon.getTestList()};
_back = Basic.convertToBase64( JsonConvert.SerializeObject(arr) );
```
*顺便说下，我对加密的认识也就是到“知道MD5是什么”这种程度而已，本作品中交流的数据都转成Base64，这之上的手段并未有研究。*

###<p id="database2">数据库连接</p>

学生侧的数据库连接还需要实现这个接口：
``` c#
interface IStudentDbCon
    {
        String getQuestion(String testname);     
        int getTestTime(String testname);
        /**获取试卷testname的答案，只是用作批改，并不会发回给客户端*/
        List<int> getAnswer(String testname);
//单论写入一个分数数据是不需要有班级属性，但是为了数据库做统计时生成映射要有班级，
所以写入分数的函数做了两个，客户端不需要发送带班级的信息，setScore会获取学生班级然后调用setScoreByClass
        void setScore(string testname, string number, int score);
        void setScoreByClass(string testname, string number, string classs, int score);
    }
```

向数据库写入成绩的代码如下：
``` c#
var col = mcon.GetCollection("Score:" + name);
if (!col.Exists())//没有这个集合时新建&索引
{
       var str = BsonJavaScript.Create("addIndexForScore(" + name.ToJson() + ")" );
       mcon.Eval(EvalFlags.None, str);
}
col.Save
(
     new BsonDocument
		        {
		            {"_id", number},
                    {"classs",classs},
		            {"score", score}
		        }
);
DbBasic.rwHt[name] = DbBasic.STAGE_WRITE;
```

###cookies

做Socket版的时候，发送Socket需要附上“这是谁发来的、发来干什么”这种信息，比较麻烦，在mvc中用cookies来记录以下信息：学生学号、正在做的试卷、作答时间（判断是否超时）。教师侧并没有做这部分。

操作cookies的类有以下方法：
``` c#
public interface IStudentCk
	{
		void login(StudentInfo student);
		Boolean verify();
		/**记录学生的考试信息 , 参数字符串必须是 “{试卷名}&&{限制时间}” 这样的格式*/
		void testChosen(String str);
		Boolean isOverTime();
		void logout();
		/**发来请求的学生的学号*/
		String number{get;}
		/**学生正在做的试卷名*/
		String testName{get;}
		String getCkProp(String prop);
	}
```
verify使用SessionID检测用户有没有重复登录之类

判断学生做卷子有没有超时的代码如下：
``` c#
//写入时间函数
ck["test"] = HttpUtility.UrlEncode("name");
//向cookie写入中文的值时先编码
ck["endTime"] = DateTime.Now.AddMinutes(time).ToString();
...

//判断函数
DateTime end = Convert.ToDateTime( getCkProp("endTime") );
TimeSpan t = end-DateTime.Now;
if( t.Minutes>0 )return false;
return true;
```
通过设置cookie的expire属性，当用户在某个阶段的操作已经过期后，获取不到相应cookie就就返回相关信息。

并没有做的很深入的功能，只是简化了客户端加工发送信息的步骤。


##<p id="teacher">教师侧</p>

###controller
教师侧控制器有4个动作：
``` c#
/**教师登陆，发回试卷列表*/
String login();
/**发回班级列表，post请求应当是1个试卷名的字符串*/
String classListRequest();
/**发回分数，post请求是1个数组的json字符串，有2个元素[0]试卷名，[1]班级名*/
String scoreRequest();
/**发回统计结果，post请求是1个试卷名的字符串*/
String statRequest();
```
教师侧的难点在于连接数据库做统计数据，这点在数据库相关的文档中有更详细的说明。

登陆时只会返回有成绩的试卷的列表，在收到试卷请求后也只返回有做这份试卷的班级的列表。其实都是很简单的操作：
``` c#
            var cols = mcon.GetCollectionNames();
            var tmpList = new List<String>();
            #region 搜索在mongodb有试卷分数的试卷key
            foreach (var i in cols)
            {
                if (i.StartsWith("Score:"))
                {
                    var tmp = i.Remove(0, 6);
                    tmpList.Add(tmp);
                }
            }
            #endregion
```
一份试卷一个collection，命名格式都是“Score:XXX”，这里的方法是获取全部collection的名称，扫描符合这格式的collection就是试卷key，之后转换为试卷名称发送给客户端：
``` c#
           for (var k = 0; k < tmpList.Count; k++)
            {
                foreach (DictionaryEntry j in DbBasic.testHT)
                {
                    if (tmpList[k] == j.Value.ToString())
                    {
                        tmpList[k] = j.Key as String;
                    }
                }
            }
```
稍微百度了下哈希表好像没有直接用value得到key的函数，这个应用中是用试卷名和数据库key都是唯一的，但是用前者获取后者情况多得多。

要获取有这份试卷成绩的班级列表，因为成绩数据是包含班级的，只要用distinct就可以：
``` c#
var col = mcon.GetCollection("Score:" + getTestAddr(testname));
var result = col.Distinct<String>("classs") as List<String>;
```

##socket
最初做这个作品时用的就是socket通信，网上教程相当好找所以“就决定是你了！”这么想着就把精灵球扔了出去。

*这时的做法是每个socket连接开一个Thread进行监听。*

之后被告知了异步通信以及X包问题之后开始寻找其他做法，找到最多的当然是聊天室的教程，但是尝试一下发现，（网上找来的代码）总是不会在接受完整条信息时马上给出“finish receiving”的信号，而要等客户端连接断开才会再一次endReceive然后结束接收，不是很符合这个应用的做法（？）。

直接在MVC版中继续做这部分，想要尽量做到只增不改，一条socket信息是"[headher]*(@xN)*body*"这样的字符串，body和MVC版是一样的的，因此只是增加了监听连接和解析header的内容，处理并返回请求依然是用的MVC的方法，~~无视掉摸索的时间~~花了1天就完成了比较轻松，但是mvc中会给连接的用户cookie写入sessionID之类的信息，socket中并没有~~所以安全性更弱了~~。

异步接收对象继承了网上的StateObject，增加了分别接受header和body并直接返回是否已经接收完毕的Boolean。
``` c#
       public override Boolean appendMsg(int readLen)
        {
            String msg = Encoding.Default.GetString(buffer, 0, readLen);
            if (header == null) 
            {
                header = SoHeaderInfo.convertToObject(msg);
                bytesToReceive = header.bodyLength;
            }
            else
            {
                sBuilder.Append(msg);
                bytesToReceive -= readLen;
            }
            return bytesToReceive == 0;
        }
```
返回true时就是数据接收完毕，不用等客户端断开。

header是5个元素的数组,包含客户端类型（学生0或教师1），body是什么信息、发送者账号和标记码和body长度
``` c#
public Char client;
public int type;
public String number;
public int bodyLength;
public static SoHeaderInfo convertToObject(String json)
        {
            var i = json.Length-1;
            Regex rgx = new Regex("@+\\z");
            json = rgx.Replace(json, "");
            String[] h = JsonConvert.DeserializeObject<String[]>(json);
            return new SoHeaderInfo(
                 int.Parse(h[0]),
                int.Parse(h[1]),
                h[2],
                h[3],
                int.Parse(h[4])
                );
        }
```
*type指示的是body是试卷请求还是统计数据请求什么的，之后根据这个值做不同处理。*


异步通信部分代码：
``` c#
           socket.BeginReceive(state.buffer, 0, HEADER_LENGTH, 
                SocketFlags.None, new AsyncCallback(callBack_Receive), state);
           ···
           Socket so = state.workSocket;
            int bytesRead = so.EndReceive(ar);
            if (bytesRead > 0)
            {
                Boolean tmp = state.appendMsg(bytesRead);
                if (!tmp)
                {
                    so.BeginReceive(state.buffer, 0, state.bytesToReceive, 0,
                        new AsyncCallback(callBack_Receive), state);
                    return;
                }
            }
            receivedMsgHandler(state);           
```
继承的类override receivedMsgHandler方法来对收到的信息作不同的处理。

之后的事情的大致上都能用回MVC版的方法。

Home控制器下的openSocket和closeSocket负责控制Socket监听，调试程序是访问该Action即可。

##<p id="heartbeat">心跳检测<p/>

心跳检测分别做了mvc和socket两部分。

写了用户管理器类，成员有一个用户list，每个一段时间扫描用户的登陆状态，过期的就删掉，同时用户传来数据时也会先在这里检查一下。当用户开始考试时会延长过期时间，这样就可以不用在做题时意义不明地开着网络了。

用户list的元素是自己写的Client对象。有4个属性：账号、特征码、过期时间、在线状态，socket版的继承了这个加了点别的东西。

###<p id="hbmvc">MVC<p/>

因为mvc有很多信息都写在了cookie里，什么时候要让一个用户过期掉也是由操作cookie的部分来决定，因此mvc的管理器功能比较简单。

重复登陆的情况，会将前1个登陆的用户ban掉，同时大幅延长这个用户的过期时间来等待对方发送请求时告诉对方登陆有异常。

```
   /// <summary> 登录用户管理器基础功能接口 </summary>
    public interface IClientMgr
    {
        void Login(String num, String sid);
        void Logout(String sid);
        void Delete(String sid);
        Boolean isLogin(String sid);
        Boolean isBan(String sid);
        /// <summary> 接收到心跳包时更新登录用户的检测时间 </summary>
        void heartBeat(String sid);
        void startHeartBeatTest();
        void stopHeartBeatTest();
    }
```

MVC使用的Client对象比较简单，状态也只有 “正常”、“被踢除”和“过期”3种。

在mvc版里过不过期用cookie的Expire设定，因此实际上只用到2种状态。

麻烦的部分都在cookie操作里面
```as3
 public override String verify()
        {
            HttpCookie ck = getCookie("student");
            //没有student cookie的情况：此用户从未登录
            if (ck == null)...

            String sid = ck["sid"];
            //此客户已被踢出，可能因为有其他人用同一账号登陆
            if (LoginClientMgr_MVC.instance.isBan(sid))...

            // 不在管理器login数组中的情况：此用户过长时间没有链接被删除
            if (!LoginClientMgr_MVC.instance.isLogin(sid))...

            ck = getCookie("heartbeat");
            //超时未心跳检测
            if (ck == null)...

            //验证通过，不在考试期间才会更新验证时间
            if (getCookie("test") == null)...
            return Basic.StatusMessage.MSG_VERIFY_SUCCESS;
        }
```
做完之后一看这样混着用cookie和clientMgr检测用户在线情况的做法真是诡异，但是一时间也想不到好方法了。

###<p id="hnsocket">Socket</p>

Socket的部分则要复杂的多，比起mvc增加了记录考试信息等等的功能，在考试延长了过期时间之后超时交卷就会记为0分，不交卷再次过期的话则就变成一般的过期状态然后删除。

此外socket版的没有了SessionID，这里~~很随便的~~用“登陆账号-登录时间”作为每个用户的特征码。

用户状态也更多了：
``` as3

 foreach (var i in _list)
            {
                if (!i.isOnline())
                {
                    switch (i.status)
                    {
                        //正常在线中→过时→超时未心跳检测
                        case Client_Student.STATUS_ONLINE:
                        //考试超时→再次过期→超时未心跳检测
                        case Client_Student.STATUS_TEST_OVERTIME:
                            ...
                            break;
                        //检测过期后再次过期→删除 
                        case Client_Student.STATUS_OVERTIME:
                        // 被踢出后过期→删除
                        case Client_Student.STATUS_BAN:
                           ...
                            break;
                        //正在考试中→过期→考试超时
                        case Client_Student.STATUS_TESTING:
                           ...
                            break;
                    }
                }

```

用户list的成员改为了继承Client的Client_Student，加入了储存考试信息、切换在线状态的方法。

于是在用户管理器的模板类中，为了应对2种用户的检索需要，在本设计中~~貌似~~第一次自己写了泛型方法：
```
protected virtual List<T> searchByNumber<T>(String num) where T : Client
        {
            var tmp = _list.FindAll((cli) => { return cli.number == num; });
            var ret = tmp.ConvertAll<T>(x => (T)x).ToList();
            return ret;
        }

 protected virtual T searchBySid<T>(String sid) where T : Client
        {
            return (T)_list.Find((cli) => { return cli.sid == sid; });
        }
```





