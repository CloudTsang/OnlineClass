using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;
namespace TestOnlineMVC.Controllers
{
	/// <summary> 处理POST请求的控制器 </summary>
	public class PostController_Template : Controller
	{
		protected byte[] GetPostDataBuffer()
		{
			Request.InputStream.Position = 0;
			var buffer = new byte[Request.InputStream.Length];
			Request.InputStream.Read(buffer, 0, buffer.Length);
			Request.InputStream.Position = 0;
			return buffer;
		}
		///<summary>获取post请求的字符串*</summary>
		protected string GetPostStr()
		{
			return Encoding.UTF8.GetString(GetPostDataBuffer());
		}
		///<summary>获取解析post请求的json字符串得到的T类型实例</summary>
		protected T GetReq<T>()
		{
			return JsonConvert.DeserializeObject<T>(GetPostStr());
		}
		
/*		
        protected String getPostMsg(){
			try{
				using(Stream s = Request.InputStream){
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