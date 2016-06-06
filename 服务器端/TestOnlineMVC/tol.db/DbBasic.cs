using System;
using System.Collections;
using CSRedis;
using MongoDB.Driver;

namespace TestOnlineMVC.tol.db
{
	public class DbBasic
	{
		///<summary>试卷名和数据库key的哈希表</summary>
		public static Hashtable testHT;

		internal  const int STAGE_READ = 1;
		internal  const int STAGE_WRITE = 2;
		///<summary>试卷数据读写状态标记，有人写入时置STAGE_WRITE，读出时置STAGE_READ , 
		/// 在STAGE_WRITE时读出会更新读出项在db中的结果</summary>
		public static Hashtable rwHt;

		///<summary>生成MongoDB连接字符串</summary>
		internal static String buildMongoConnect(String user, String pw,
		                                       String host = "localhost", String port = "27017", String db = "admin")
		{
			var str = String.Format("mongodb://{0}:{1}@{2}:{3}", user, pw, host, port);
			if (db != "admin") str += "/" + db;
			return str;
		}

		internal static void createRwHt(MongoDatabase mcon, Boolean isnew = false)
		{
			if (isnew == false && rwHt != null) return;
			rwHt = new Hashtable();
			var cols = mcon.GetCollectionNames();
			foreach (var i in cols)
			{
				if (i.StartsWith("Score:"))
				{
					var tmp = i.Remove(0, 6);
					rwHt.Add( tmp , STAGE_READ );
				}
			}
		}

		///<summary>记录试卷名和数据库key的映射 , 参数isnew是否新建哈希表,rcon：redis数据库连接</summary>
		internal static void createTestHT(RedisClient rcon , Boolean isnew=false)
		{
			if (isnew == false && testHT != null) return;
			testHT = new Hashtable();
			foreach (var element in rcon.HGetAll("TestName"))
			{
//			    Basic.trace(element.Key+"   "+element.Value);
				testHT.Add(element.Key , element.Value);
			}
		}
	}
}