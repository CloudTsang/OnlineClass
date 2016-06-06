using System;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace TestOnlineMVC
{
    public class Basic
    {
        ///<summary>Base64 = "错误的请求"</summary>
        public const String ERR = "6ZSZ6K+v55qE6K+35rGC";
        public const int APP_PORT = 9985;
        /// <summary> 心跳检测间隔(min) </summary>
        public const double HB_SPAN = 1;
        /// <summary> 信息类型码 </summary>
        public enum dataType
        {
            登录口令 = 0,
            登陆成功信息 = 1,
            用户资料 = 2,
            答案 = 3,
            题目 = 4,
            分数 = 5,
            教师端登录 = 6,
            试题列表 = 7,
            试卷班级列表 = 8,
            教师端分数信息 = 9,
            统计信息 = 10,
            分数段人数统计信息 = 11,
            其他请求 = 12,
            错误信息 = 13,
            放弃作答 = 14,
            心跳检测信息 = 15,
            登出 = 16
        }

        /// <summary> 连接状态返回信息 </summary>
        public class StatusMessage
        {
            public const String MSG_VERIFY_SUCCESS = "success";
            public const String MSG_NOT_LOGIN = "请先登录。";
            public const String MSG_ANOTHER_LOGIN = "您的账号在别处登陆了！";
            public const String MSG_HBT_OVERTIME = "网络连接已断开请重新登陆。";
        }

        public static void trace(Object value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }

        public static String getIP()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipa = ipHost.AddressList[1];
            return ipa.ToString();
        }

        public static string HTTPRequestIpAddr
        {
            get { return HttpContext.Current.Request.UserHostAddress; }
        }

        ///<summary>普通字符串转Base64*</summary>
        public static String convertToBase64(String str)
        {
            byte[] barr = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(barr);
        }
        ///<summary>Base64转普通字符串*</summary>
        public static String convertFromBase64(String str)
        {
            byte[] barr = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(barr);
        }
    }
}
