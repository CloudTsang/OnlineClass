##网络课程APP服务器端

* [简介](#1)
 * [获取POST请求数据](#获取POST请求数据) 
 * [数据库连接](#数据库连接)
 * [登陆信息](#登陆信息)
* [学生侧](#学生侧)
 * [controller](#controller)
 * [请求处理](#请求处理)
 * [数据库连接](#数据库连接)
 * [cookies](#cookies)
* [教师侧](#教师侧)

从0学起用C#写的服务器，刚开始使用Socket，之后改成了用MVC。

C#和AS3比就是格式不太一样，学起来没有很费心思，当然我还有很多没有触及的地方，因为开始只是为了做毕设学的C#，用不到的功能就没太管。

通过nuget、各种引用扩展功能的方便与自由可谓远超AS3的强大，这个作品只需从nuget安装以下程序包
* Newtonsoft.Json
* csredis //完全无料没有读写条数限制的redis驱动，功能是很简单~~就是redis命令行~~但也够用。~~懒得找旧版本的servicestack.redis~~
* （Legacy） C# Driver for MongoDB //提供旧版的MongoDB操作方法。

此外多线程是AS3没有的，我还不太会操作这个，什么时候要lock什么变量之类，做Socket开侦听时有接触，MVC中则没有使用。

---
##<p id="1">简介</p>

主要工作流程是用户访问控制器的动作，服务器将信息传给事件处理器EvtHandler，EvtHandler处理之后设置一个返回信息sendBack，控制器return这个字符串。


```
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

###获取POST请求数据
这里没有烦直接百度了一段
```
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

###数据库连接
无论处理来自学生还是教师的请求，都需要向数据库请求的数据是学生信息和试卷列表。
```
    interface IDbConBase
    {
        /**获取1个学号为no的学生资料*/
        StudentInfo getStudent(String no);
        /**获取试卷列表*/
        String getTestList();
    }
```
试卷列表放在mongodb中，虽然不像redis时只是一条json拿出来就直接发客户端，但是驱动提供了相应方法简化这些操作：
```
public virtual String getTestList()
        {
//          return rcon.Get("TestList");
//使用redis的情况就只是这么1句
            var col = mcon.GetCollection("TestList");
            var list = col.FindAll().ToJson();
            return list;
```
还需要创建试卷名和数据库key的哈希表，这个表是静态的，在不为null时传入true的参数才会重新创建一个表
```
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

###登陆信息
用户身份信息有三种：客户端发来的账号密码、教师信息、学生信息，写起来刚好是后一个继承前一个，但教师侧并没有做登陆验证，因此教师信息没有使用，也没有加进数据库。
```
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


学生侧
---

####controller
学生侧的控制器要有以下Action：
```
public virtual String login(String info=null);
//客户端可以用GET或POST测试，所以控制器里写了默认为null的参数。
public virtual  String testRequest(String info=null);
public  virtual String answerCheck(String info=null);
```
####请求处理
学生侧需要处理的请求有3个：登录验证、发送试卷题目、批改并返回成绩。
```
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
```
string[] arr = {ser.ClientSide.convertToJson() , _dbcon.getTestList()};
_back = Basic.convertToBase64( JsonConvert.SerializeObject(arr) );
```
*顺便说下，我对加密的认识也就是到“知道MD5是什么”这种程度而已，本作品中交流的数据都转成Base64，这之上的手段并未有研究。*

###数据库连接

学生侧的数据库连接还需要实现这个接口：
```
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
```
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
```
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
verify使用SessionID检测用户有没有重复登录之类，但是检测用户是否在线的心跳没有做，所以logout没有用，verify也没有意义（不能删除登出的用户信息，测试时会变成重复登录）。

判断学生做卷子有没有超时的代码如下：
```
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
并没有做的很深入的功能，只是简化了客户端加工发送信息的步骤。

教师侧
---
###controller
教师侧控制器有4个动作：
```
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
```
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
```
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

要获取有这份试卷成绩的班级列表，因为成绩数据是包含板技术新的，只要用distinct就可以：
```
var col = mcon.GetCollection("Score:" + getTestAddr(testname));
var result = col.Distinct<String>("classs") as List<String>;
```







