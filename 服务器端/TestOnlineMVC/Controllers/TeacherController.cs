using System;
using System.Web.Mvc;
using TestOnlineMVC.tol.filter;
using TestOnlineMVC.tol.net;
namespace TestOnlineMVC.Controllers
{
	/// <summary> 教师端控制器 - GET请求</summary>
    [CryptFilter_GET(Order = 1)]
    public class TeacherController : TemplateTeacherController
	{
	}
}