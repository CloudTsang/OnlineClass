/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/4/18
 * 时间: 11:23
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using CSRedis;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
namespace QuestionInput
{
	public class DbCon
	{
		protected const String MONGO_CONNECT_STRING = "mongodb://administrator:password@localhost:27017";
		protected RedisClient redis;
		protected MongoClient mClient;
		protected MongoServer mServer;
		protected MongoDatabase mDB;
		public DbCon()
		{
			redis=new RedisClient("localhost");
			redis.Auth("password");
			
			mClient = new MongoClient(MONGO_CONNECT_STRING);
			mServer = mClient.GetServer();
			mDB = mServer.GetDatabase("db1");
			
		}
		
		public void saveQuestion(String key , String question){
			redis.Set("QuestionLib:"+key , question);
		}
		
		public void saveTestList(String testname , String key , String des){
			MongoCollection col = mDB.GetCollection("TestList");
			try{
				redis.HSet("TestName" , testname , key);
			}catch(Exception e){
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
			BsonDocument doc = new BsonDocument{
				{"_id",testname},
				{"description" ,des}
			};
			col.Save(doc);
		}
		
		public void saveAnswer(String key , String ans){
			redis.Set("AnswerLib:"+key  , ans);
		}
		
		public void saveTime(String key , int time){
			try{
				redis.HSet("TestTime" , key , time);
			}catch(Exception e){
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
		}
		public void saveAddr(String name , String key){
			try{
				redis.HSet("TestName" , name , key);
			}catch(Exception e){
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
		}
		public void disconnect(){
			redis.Quit();
			redis.Dispose();
			
		}
	}
}
