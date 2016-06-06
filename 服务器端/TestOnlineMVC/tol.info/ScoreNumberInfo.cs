using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TestOnlineMVC.tol.info
{
    ///<summary>分数段人数统计</summary>
    public class ScoreNumberInfo : IInfo
    {
        public String _id;
        public String classs;
        public String test;
        public double[] number;
        public string convertToJson()
        {
            return JsonConvert.SerializeObject(new ScoreNumebrInfoJson(_id , number));
        }
        /// <summary>
        /// 将MongoDB的储存格式转换为发送给客户端的格式。
        /// 只有班级和（整形数组）分数段人数统计两个属性
        /// </summary>
        private class ScoreNumebrInfoJson
        {
            public int[] number;
//            public List<int> number;
            public String classs;

            public ScoreNumebrInfoJson(String c, double[] n)
            {
                classs = c;
//                number = n.Select(x => (int) x).ToList();

                number = n.Select(x => (int) x).ToArray();
//                number = Array.ConvertAll<double, int>(n, num => (int) num);
            }
        }
    }
}