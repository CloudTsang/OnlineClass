/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/4/20
 * 时间: 7:52
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace QuestionInput
{
	/// <summary>
	/// 1份试卷的数据
	/// </summary>
	public class TestInfo
	{
		public String name;
		public String dbkey;
		private String _des;
		public List<String> question;
		public List<int> answer;
		public int time;
		protected DbCon db;
		
		private String tmpQuestion;
		private String tmpAnswer;
		public TestInfo()
		{
			question = new List<string>();
			answer = new List<int>();
			
		}
		
		public void SaveTest(){
			if(db == null)db = new DbCon();
			db.saveAddr(name , dbkey);
			db.saveAnswer(dbkey , tmpAnswer);
			db.saveQuestion(dbkey , tmpQuestion);
			db.saveTestList(name , dbkey , _des);
			db.saveTime(dbkey , time);
			
		}
		
		public String createPaperJson(){
			PaperInfo info = new PaperInfo(name , time,  question);
			tmpQuestion =  info.converToJson();
			tmpAnswer = JsonConvert.SerializeObject(answer);
			return tmpQuestion;
		}
		
		public String toString(){
			createPaperJson();
			String str = String.Format("试卷名称：{0}\n试卷在数据库中的key：{1}\n试卷描述：{2}\n试卷限时：{3}",
			                           name , dbkey , _des ,time);
			str+="\n试卷题目总数："+question.Count;
			str+="\n题目信息："+ tmpQuestion;
			str+="\n答案:"+tmpAnswer;
			return str;
		}
		
		public String description{
			set{
				if(value=="") _des=String.Format("{0}条题目，限时{1}分钟完成",answer.Count,time);
				else _des = value;
			}
			get{
				return _des;
			}
		}
	}
}
