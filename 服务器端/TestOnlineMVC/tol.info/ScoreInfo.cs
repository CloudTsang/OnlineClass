using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
namespace TestOnlineMVC.tol.info
{
	/// <summary>分数信息，用于发送给教师端</summary>
	public class ScoreInfo : IInfo
	{
		private string[] _students;
		private int[] _score;
		/// <param name="numbers">学号数组</param>
		/// <param name="scores">分数数组</param>
		public ScoreInfo(string[] numbers , string[] scores)
		{
			if(numbers.Length != scores.Length) throw new Exception("Wrong param!");
			_students = numbers;
			_score = Array.ConvertAll<String , int>(scores , str=>int.Parse(str) );
		}
		
		public string[] number{
			get{
				return _students;
			}
		}
		public int[] score{
			get{
				return _score;
			}
		}
		
		public String convertToJson(){
			return JsonConvert.SerializeObject(data);
		}
		///<summary>返回一个学号数组和分数数组组成的两个元素的数组*</summary>
		public List<IList> data{
			get{
				List<IList> tmp = new List<IList>{_students , _score};
				return tmp;
			}
		}
	}
}
