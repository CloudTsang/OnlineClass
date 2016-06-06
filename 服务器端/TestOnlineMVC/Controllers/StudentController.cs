using System;
using TestOnlineMVC.tol.coder;
using TestOnlineMVC.tol.filter;
using TestOnlineMVC.tol.net;

namespace TestOnlineMVC.Controllers
{
    /// <summary>
    /// 处理学生端GET类型请求的控制器
    /// </summary>
    [CryptFilter_GET( TolCoderSt.CoderNum.Base64 , TolCoderSt.CoderNum.Base64 , Order = 1)]
    public class StudentController : TemplateStudentController
    {
    }
}