using System;
using CSRedis;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.db
{
    public class DbConBase:IDbConBase
    {
        protected const String MONGO_CONNECT_STRING = "mongodb://localhost:27017";
        protected const String DB_ADDR = "localhost";
        protected const String DB_PASSWORD;

        protected RedisClient rcon;
        protected MongoDatabase mcon;

        /// <summary>
        /// 基础数据库连接
        /// </summary>
        /// <param name="mdb">mongo数据库</param>
        /// <param name="mcom">mongo连接地址</param>
        /// <param name="rhost">redis连接地址</param>
        /// <param name="rpw">redis连接密码</param>
        public DbConBase(String mdb , String mcom  , String rhost , String rpw )
		{
			rcon=new RedisClient(rhost);
			rcon.Auth(rpw);
            var mClient = new MongoClient(mcom);
            var mServer = mClient.GetServer();
            mcon = mServer.GetDatabase(mdb);          
            DbBasic.createTestHT(rcon);
            DbBasic.createRwHt(mcon);
		}

        public StudentInfo getStudent(string no)
        {
            var col = mcon.GetCollection("StudentFile");
            var query = Query.EQ("number", no);
            var info = col.FindOneAs<StudentInfo>(query);
            return info;
        }

        ///<summary>获取试卷列表的json字符串（服务器端无需解析该json，获取字符串后直接发送给客户端即可）</summary>
        public virtual String getTestList()
        {
            var list = rcon.HGetAll("TestList").ToJson();
//            var col = mcon.GetCollection("TestList");
//            var list = col.FindAll().ToJson();
            return list;
        }

        public string getTestAddr(string testname)
        {
            return DbBasic.testHT[testname] as String;
        }

        public void renewTestNameTable()
        {
            DbBasic.createTestHT(rcon,true);
        } 
    }
}