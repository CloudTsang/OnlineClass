using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace TestOnlineMVC.tol.info
{
    public class TestInfo : IInfo
    {
        [JsonIgnore]public ObjectId _id;
        public String name;
        public String description;

        public String convertToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}