using System;

namespace TestOnlineMVC.tol.socketserver
{
    /// <summary>socket数据处理接口</summary>
    interface IDataProcessor
    {
        String dataProcess(ISocketMsg msg);
    }
}