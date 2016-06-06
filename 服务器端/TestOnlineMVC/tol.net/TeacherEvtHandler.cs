using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Newtonsoft.Json;
using TestOnlineMVC.tol.coder;
using TestOnlineMVC.tol.db;
namespace TestOnlineMVC.tol.net
{
	/// <summary> 教师端请求处理 </summary>
	public class TeacherEvtHandler
	{
	    private static TeacherDbCon _dbcon;
		private String _back;
		public TeacherEvtHandler(DbConBase db=null)
		{
			if(db!=null) _dbcon = db as TeacherDbCon;
            else _dbcon = TeacherDbCon.instance;
		}
		
		public void loginHandler(){
			_back = _dbcon.getTestList();
		}

        public void scoreHandler(String info)
        {
            String[] tmp = JsonConvert.DeserializeObject<String[]>(info);
            Basic.trace("准备发送试题 " + tmp[0] + " 的 " + tmp[1] + " 班级学生分数。");
            BsonArray ret = _dbcon.getScore(tmp[0], tmp[1]);
            _back = ret.ToJson();
        }

	    public void classsHandler(String info)
	    {
            Basic.trace("准备发送做了试题 " + info + " 的班级的列表。");
	        List<String> ret = _dbcon.getClassList(info);
	        _back = JsonConvert.SerializeObject(ret);
	    }

	    public void statHandler(String info)
	    {
	        var stat = _dbcon.getStat(info);
	        var ret = new List<String>();
	        ret = stat.Select(x => x.convertToJson()).ToList();
	        _back = JsonConvert.SerializeObject(ret);
	    }

		public String sendBack{
			get{
				return _back;
			}
		}
	}
	
	
}
