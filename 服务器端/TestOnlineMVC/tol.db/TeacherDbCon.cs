using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TestOnlineMVC.tol.info;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace TestOnlineMVC.tol.db
{
    public class TeacherDbCon : DbConBase, ITeacherDbCon
    {
        private static TeacherDbCon _ins;
        public TeacherDbCon(String mdb = "db1", String mcom = MONGO_CONNECT_STRING, String rhost = DB_ADDR, String rpw = DB_PASSWORD)
            : base(mdb, mcom, rhost, rpw)
        {
        }

        public static TeacherDbCon instance
        {
            get
            {
                if (_ins == null) _ins = new TeacherDbCon();
                return _ins;
            }
        }

        public override string getTestList()
        {
            System.Diagnostics.Debug.WriteLine("从数据库获取有成绩的试卷列表...");
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

            #region 获取搜索结果的试卷名称
            for (var k = 0; k < tmpList.Count; k++)
            {
                foreach (DictionaryEntry j in DbBasic.testHT)
                {
                    if (tmpList[k] == j.Value.ToString())
                    {
                        tmpList[k] = j.Key as String;
                        continue;
                    }
                }
            }
            #endregion
            return JsonConvert.SerializeObject(tmpList);
        }

        public List<String> getClassList(string testname)
        {
            //            var bjs = BsonJavaScript.Create(String.Format("getClassNumber({0})", testname));
            //            var result = mcon.Eval(EvalFlags.NoLock , bjs).ToJson();
            var col = mcon.GetCollection("Score:" + getTestAddr(testname));
            var result = col.Distinct<String>("classs") as List<String>;
            return result;

        }


        public BsonArray getScore(string testname, string classs)
        {
            System.Diagnostics.Debug.WriteLine("从数据库获取试卷{0}的{1}的学生分数", testname, classs);
            var name = getTestAddr(testname);
            var js = BsonJavaScript.Create(String.Format("getStudents('{0}','{1}')", name, classs));
            var result = mcon.Eval(EvalFlags.NoLock, js);
            return result.AsBsonArray;
        }

        public IInfo getScoreNumber(String testname, String classs = "all")
        {
            MongoCollection collection;
            var name = getTestAddr(testname);
            if ((DbBasic.rwHt[name] as int?) == DbBasic.STAGE_READ)//处于读阶段时，尝试直接读取数据库中的统计结果
            {
                collection = mcon.GetCollection("ChartStat");
                var query = Query.And(Query.EQ("_id", classs), Query.EQ("test", name));
                var tmp = collection.FindOneAs<ScoreNumberInfo>(query);
                if (tmp != null) return tmp;
            }
            //处于写阶段时、没有统计结果时，调用数据库的StatScoreNumber2函数进行分数段人数统计
            collection = mcon.GetCollection("ChatStat");
            var str = String.Format("StatScoreNumber2('{0}','{1}')", name, classs);
            var tmp2 = mcon.Eval(EvalFlags.NoLock, BsonJavaScript.Create(str));
            var result = JsonConvert.DeserializeObject<ScoreNumberInfo>(tmp2.ToJson());
            collection.Save(tmp2);
            return result;
        }

        public List<StatInfo> getStat(String testname)
        {
            List<StatInfo> list = new List<StatInfo>(); ;
            MongoCollection collection;
            var name = getTestAddr(testname);
            System.Diagnostics.Debug.WriteLine(name);
            if ((DbBasic.rwHt[name] as int?) == DbBasic.STAGE_READ)//处于读阶段时，尝试直接读取数据库中的统计结果
            {
                System.Diagnostics.Debug.WriteLine("尝试从数据库获取试卷 " + testname + " 的统计结果");
                collection = mcon.GetCollection("StatResult:" + name);
                if (collection != null)
                {
                    try
                    {
                        list = collection.FindAllAs<StatInfo>().ToList();
                    }
                    catch (Exception e)
                    {
                        Basic.trace(e.StackTrace);
                    }
                    if (list.Count() != 0)
                    {
                        return list;
                    }
                }
            }
            //处于写阶段时、没有统计结果时，使用mapreduce进行统计
            System.Diagnostics.Debug.WriteLine("数据库进行试卷 " + testname + " 的分数统计");
            collection = mcon.GetCollection("Score:" + name);
            String map = "function(){emit(this.classs,this.score)}";
            String map2 = "function(){emit('总体',this.score)}";
            String reduce = "function(key , values){var result = renewStat2(key , values," + name.ToJson() + ");return result;}";

            MapReduceArgs args = new MapReduceArgs();
            args.MapFunction = BsonJavaScript.Create(map);
            args.ReduceFunction = BsonJavaScript.Create(reduce);
            args.OutputCollectionName = ("StatResult:" + name);
            args.OutputDatabaseName = mcon.Name;
            //            args.OutputMode = MapReduceOutputMode.Replace;   
            //            args.OutputMode = MapReduceOutputMode.Inline;
            args.OutputMode = MapReduceOutputMode.Merge;

            var md = collection.MapReduce(args);
            var ms = md.GetResultsAs<StatInfo>();
            list = ms.ToList();

            args.MapFunction = BsonJavaScript.Create(map2);
            md = collection.MapReduce(args);
            ms = md.GetResultsAs<StatInfo>().ToList();
            list.Concat(ms);
            //                                foreach (var i in mresult.GetResultsAs<StatInfo>()) list.Add(i.convertToJson());
            DbBasic.rwHt[name] = DbBasic.STAGE_READ;
            return list;

        }
    }
}