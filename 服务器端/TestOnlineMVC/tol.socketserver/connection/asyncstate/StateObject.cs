using System;
using System.Net.Sockets;
using System.Text;

namespace TestOnlineMVC.tol.socketserver.connection.asyncstate
{
    public abstract class StateObject
    {
        ///<summary>通信socket本体</summary> 
        public Socket workSocket;
        ///<summary>接收信息</summary> 
        public byte[] buffer = new byte[1024];
        ///<summary>信息转文字</summary> 
        public StringBuilder sBuilder = new StringBuilder();
        ///<summary>将信息写入buffer，readLen是被写入信息的长度, 返回Boolean为body是否已读取完毕</summary>
        public abstract Boolean appendMsg(int readLen);
    }
}