using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TestOnlineMVC.tol.info
{
    /// <summary> Socket头信息 </summary>
    public class SoHeaderInfo : IInfo
    {
        ///<summary>用户类型</summary>
        public int client;
        ///<summary>数据类型</summary>
        public int type;
        ///<summary>发送者账号</summary>
        public String number;
        ///<summary>发送者标识码</summary>
        public String sid;
        ///<summary>主题字节数</summary>
        public int bodyLength;
        /// <summary>
        /// 创建头文件对象
        /// </summary>
        /// <param name="c">客户端类型，只有0（tudent）和1（eacher）两个值</param>
        /// <param name="t">信息类型</param>
        /// <param name="n">发送者账号，学生侧客户端才要求登录账号密码验证，教师端传过来的是“****”</param>
        /// <param name="s">发送者标识码</param>
        /// <param name="l">主体字节数</param>
        public SoHeaderInfo(int c , int t , String n ,String s, int l)
        {
            if(c<0 || c>1) throw  new Exception("Wrong param!");
            client = c;
            type = t;
            number = n;
            sid = s;
            bodyLength = l;
        }

        public string convertToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// 将Json字符串转换为头文件信息
        /// </summary>
        /// <param name="json">这个Json字符串应当是一个按顺序包含以下元素的数组：
        /// <remarks>发送信息的客户端类型(0 or 1)</remarks>
        /// <remarks>信息类型</remarks>
        /// <remarks>发送账号</remarks>
        /// <remarks>发送者标识码</remarks>
        /// <remarks>主体字节数</remarks>
        /// </param>
        public static SoHeaderInfo convertToObject(String json)
        {
            var i = json.Length-1;
            Regex rgx = new Regex("@+\\z");
            json = rgx.Replace(json, "");
//            Basic.trace("header : "+json);
            String[] h = JsonConvert.DeserializeObject<String[]>(json);
            return new SoHeaderInfo(
                int.Parse(h[0]),
                int.Parse(h[1]),
                h[2],
                h[3],
                int.Parse(h[4])
                );
        }
    }
}