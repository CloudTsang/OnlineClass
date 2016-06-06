using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Text;
namespace TestOnlineMVC.tol.net
{
	///<summary>获取post信息</summary>
	public class PostMsgGetter
	{
		public static byte[] GetPostDataBuffer()
		{
			HttpContext.Current.Request.InputStream.Position = 0;
			var buffer = new byte[HttpContext.Current.Request.InputStream.Length];
			HttpContext.Current.Request.InputStream.Read(buffer, 0, buffer.Length);
			HttpContext.Current.Request.InputStream.Position = 0;
            Basic.trace(HttpContext.Current.Request.UserHostAddress);
			return buffer;
		}
		///<summary>获取post请求的字符串*</summary>
		public static string GetPostStr()
		{
			return Encoding.UTF8.GetString(GetPostDataBuffer());
		}
		///<summary>获取解析post请求的json字符串得到的T类型实例</summary>
		public static T GetReq<T>()
		{
			return JsonConvert.DeserializeObject<T>(GetPostStr());
		}

        /*
		public static String getPostMsg(){
			try{
				using(Stream s = HttpContext.Current.Request.InputStream){
					int pos = 0;
					byte[] buffer = new byte[1024];
					StringBuilder builder = new StringBuilder();
					while ( (pos = s.Read(buffer , 0 , 1024)) >0) {
						builder.Append( Encoding.UTF8.GetString(buffer,0,pos) );
					}
					return builder.ToString();
				}
			}
			catch(Exception err){
				Console.WriteLine(err);
			}
			return ("Getting post message failed. ");
		}
         */
	}
}
