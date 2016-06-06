using System;
using System.Web.Mvc;
using TestOnlineMVC.tol.coder;
using TestOnlineMVC.tol.filter;
using TestOnlineMVC.tol.net;
namespace TestOnlineMVC.Controllers
{
	/// <summary>
	///  Post类型教师控制器
	/// </summary>
   [CryptFilter_POST(Order = 1)]
	public class PTeacherController : TemplateTeacherController
	{
	}
}