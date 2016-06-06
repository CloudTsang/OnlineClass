using System;
using Newtonsoft.Json;

namespace TestOnlineMVC.tol.info
{
    /// <summary> 登录用户的基础信息,包含用户账号&特征码 </summary>
    public class ClientInfo:IInfo
    {
        protected String _num;
        protected String _sid;

        public ClientInfo(String n, String id)
        {
            _num = n;
            _sid = id;
        }
        ///<summary>用户账号</summary>
        public string number
        {
            get { return _num; }
        }

        ///<summary>特征码，使用mvc时为SessionID，使用socket时为？</summary>
        public String sid
        {
            get { return _sid; }
        }

        public string convertToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}