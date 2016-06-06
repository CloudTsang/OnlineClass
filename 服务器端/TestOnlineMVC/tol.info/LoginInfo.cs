using System;
using MongoDB.Bson;
using Newtonsoft.Json;
namespace TestOnlineMVC.tol.info
{
	/// <summary>登陆信息</summary>
	public class LoginInfo : IInfo
	{
	    [JsonIgnore] public ObjectId _id;
		public String number;
		public String password;

		public LoginInfo(){		}

		public String convertToJson(){
			return JsonConvert.SerializeObject(this);
		}
	}
}
