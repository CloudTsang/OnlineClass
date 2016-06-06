using System;

namespace TestOnlineMVC.tol.socketserver
{
    ///<summary>Socket信息接收器必要功能接口</summary>
    public interface ISocketMsg
    {
        ///<summary>客户端类型</summary>
        int clientType { get; }
        ///<summary>信息类型</summary>
        int msgType { get; }
        ///<summary>发送者账号</summary>
        String client { get; }
        ///<summary>发送者标识码</summary>
        String specifID { get; }
        ///<summary>信息正文</summary>
        String msgBody { get; }
        ///<summary>转换信息为T类型对象</summary>
        T getOBject<T>();
    }
}
