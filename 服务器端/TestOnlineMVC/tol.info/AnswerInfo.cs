using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TestOnlineMVC.tol.db;
namespace TestOnlineMVC.tol.info
{
	/// <summary>客户端传来的答案信息。</summary>
	public class AnswerInfo
	{
		private readonly String _test;
		private readonly String _number;
		private readonly List<int> _ans;
		/// <param name="answer">答案数组</param>
		/// <param name="number">学生学号</param>
		/// <param name="test">试卷名称</param>
		public AnswerInfo(List<int> answer, String number="" ,String test="")
		{
			_test = test;
			_ans = answer;
			_number = number;
		}
		///<summary>试卷名称</summary>
		public String test{
			get{
				return _test;
			}
		}
		///<summary>学生学号</summary>
		public String number{
			get{
				return _number;
			}
		}
		///<summary>答案数组</summary>
		public List<int> answer{
			get{
				return _ans;
			}
		}
		
		/// <summary>答案批改</summary>
		/// <param name="ans">客户端传来的答案</param>
		/// <param name="test">试卷名</param>
		/// <param name="rcon">查询答案的数据库连接</param>
		/// <returns>分数：答对的多少道题</returns>
		public static double check2(List<int> ans , String test, StudentDbCon rcon){
			List<int> tmp = rcon.getAnswer(test);
//		    Basic.trace("学生答案：" + JsonConvert.SerializeObject(ans));
//            Basic.trace("正确答案：" + JsonConvert.SerializeObject(tmp));
		    int right = 0;
			for( int i=0 ; i<tmp.Count ; i++){
				if(ans[i] == tmp[i])right++;
			}
//		    Basic.trace("答对题目数：" + right + "      总题目数：" + tmp.Count);
		    double score = (double) right/tmp.Count*100;
            score = Math.Round(score,1);
//		    Basic.trace("最终考试分数：" + score);
		    return score;
		}
	}
}
