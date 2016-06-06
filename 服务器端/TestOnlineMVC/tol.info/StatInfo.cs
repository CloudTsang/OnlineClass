using System;
using System.Linq;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace TestOnlineMVC.tol.info
{
    ///<summary>分数统计结果</summary>
    public class StatInfo : IInfo
    {
        [JsonIgnore]
        public String _id;
        [JsonIgnore]
        public Object value;

        public String convertToJson()
        {
            StatInfoJson tmp;

            #region 考试成绩多于2人的正常情况
            if (value is BsonDocument)
            {
                BsonDocument b = (BsonDocument)value;
                tmp = new StatInfoJson(
                    _id,
                    b["average"].ToDouble(),
                    b["variance"].ToDouble(),
                    b["number"].AsBsonArray
                    );
            }
            #endregion

            #region 考试只有1个成绩时不能mapreduce直接返回成绩，在这里处理
            else
            {
                Double d = (Double)value;
                int[] arr = new int[5] { 0, 0, 0, 0, 0 };
                if (d >= 90) arr[0]++;
                else if (d >= 80) arr[1]++;
                else if (d >= 70) arr[2]++;
                else if (d >= 60) arr[3]++;
                else arr[4]++;
                tmp = new StatInfoJson(_id, d, 0, null, arr);
            }
            #endregion

            return JsonConvert.SerializeObject(tmp);
        }

        private class StatInfoJson
        {
            ///<summary>班级</summary>
            public String classs;
            ///<summary>平均分</summary>
            public double average;
            ///<summary>方差</summary>
            public double variance;
            ///<summary>分数段人数</summary>
            public int[] number;
            public StatInfoJson(String c, double a, double v, BsonArray n = null, int[] n2 = null)
            {
                
                classs = c;
                average = a;
                variance = v;
                if (n2 != null) number = n2;
                else
                {
                    number = new int[n.Count];
                    number = n.Select(x => x.ToInt32()).ToArray();
                }
            }
        }

    }
}