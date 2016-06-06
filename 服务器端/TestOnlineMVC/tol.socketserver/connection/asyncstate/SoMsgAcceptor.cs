using System;
using System.Text;
using Newtonsoft.Json;
using TestOnlineMVC.tol.info;

namespace TestOnlineMVC.tol.socketserver.connection.asyncstate
{
    /// <summary>真·Socket信息接收对象</summary>
    public class SoMsgAcceptor : StateObject, ISocketMsg
    {
        ///<summary>要读取的信息正文字节数</summary> 
        public int bytesToReceive;
        protected SoHeaderInfo header;

        public override Boolean appendMsg(int readLen)
        {
//            String msg = Encoding.UTF8.GetString(buffer, 0, readLen);
            String msg = Encoding.Default.GetString(buffer, 0, readLen);
            if (header == null) //读取header
            {
                header = SoHeaderInfo.convertToObject(msg);
                bytesToReceive = header.bodyLength;
            }
            else
            {
                sBuilder.Append(msg);
                bytesToReceive -= readLen;
            }
//            Basic.trace("剩余 "+bytesToReceive+" 字节未读取.");
            return bytesToReceive == 0;
        }

        public int clientType
        {
            get { return header.client; }
        }

        public int msgType
        {
            get { return header.type; }
        }

        public string client
        {
            get { return header.number; }
        }

        public string specifID
        {
            get { return header.sid; }
        }

        public string msgBody
        {
            get { return sBuilder.ToString(); }
        }

        public T getOBject<T>()
        {
            return JsonConvert.DeserializeObject<T>(sBuilder.ToString());
        }
    }
}