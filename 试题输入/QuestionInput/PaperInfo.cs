/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/4/20
 * 时间: 9:25
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace QuestionInput
{
	public class PaperInfo
	{
		public String name;
		public int time;
		public List<String> paper;
		public PaperInfo(String n , int t , List<String> p)
		{
			name = n;
			time = t;
			paper = new List<string>();
			foreach (var element in p) {
				byte[] tmp = Encoding.UTF8.GetBytes(element);
				paper.Add( Convert.ToBase64String(tmp));
			}
		}
		public String converToJson(){
			return JsonConvert.SerializeObject(this);
		}
		
	}
}
