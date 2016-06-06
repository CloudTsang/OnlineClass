using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace TestOnlineMVC.tol.info
{
    /// <summary>分数信息，用于发送给教师端(Ver. Mongo)</summary>
    public class MScoreInfo : IInfo
    {
        [JsonIgnore] public ObjectId _id;
        public string[] number;
        public int[] score;

        public MScoreInfo(){ }

        public String convertToJson()
        {
            return JsonConvert.SerializeObject(data);
        }
        ///<summary>返回一个学号数组和分数数组组成的两个元素的数组*</summary>
        public List<IList> data
        {
            get
            {
                List<IList> tmp = new List<IList> { number, score };
                return tmp;
            }
        }
    }
}