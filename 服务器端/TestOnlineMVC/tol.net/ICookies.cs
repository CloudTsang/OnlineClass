using System;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.net
{
    /// <summary>MVC侧Cookies操作基础功能接口</summary>
    interface ICookies
    {
        ///<summary>登陆成功,生成cookie</summary>
        void login(IInfo cli);
        ///<summary>账户验证</summary>
        String verify();
        ///<summary>账户登出,删除cookie</summary>
        void logout();
        ///<summary>获取名为name的用户cookie的子键prop的值</summary>
        String getCkProp(String prop, string name);
    }
}
