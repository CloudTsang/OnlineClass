using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TestOnlineMVC.tol.socketserver.connection.asyncstate;

namespace TestOnlineMVC.tol.socketserver.connection
{
	/// <summary>socket异步通信侦听器-模板</summary>
	public abstract  class SocketListenner_Template : IListener
	{
		protected ManualResetEvent reseter;
		protected Socket soListener;
		protected IPEndPoint iep;
		protected Boolean listenStatus;
		protected readonly int HEADER_LENGTH;
		public SocketListenner_Template(int len)
		{
			reseter = new ManualResetEvent(true);
			HEADER_LENGTH = len;
		}

		public void startListening(string host, int port, int backlog = 100)
		{
			iep = new IPEndPoint(IPAddress.Parse(host), port);
			soListener = new Socket(AddressFamily.InterNetwork,
									SocketType.Stream, ProtocolType.Tcp);
			soListener.Bind(iep);
			soListener.Listen(backlog);

			listenStatus = true;
			Basic.trace("Start listenning "+host+":"+port);

			while (listenStatus)
			{
				reseter.Reset();
				soListener.BeginAccept(new AsyncCallback(callBack_Accept), soListener);
				reseter.WaitOne();
			}
		}

		protected virtual void callBack_Accept(IAsyncResult ar)
		{
			//添加此命令，让主线程继续
			reseter.Set();
			// 获取客户请求的socket
			Socket listenner = ar.AsyncState as Socket;
			Socket handler = listenner.EndAccept(ar);
			var state = new SoMsgAcceptor();
			state.workSocket = handler;
//            Basic.trace("A connection from "+handler.RemoteEndPoint);
			handler.BeginReceive(state.buffer, 0, HEADER_LENGTH, 
				SocketFlags.None, new AsyncCallback(callBack_Receive), state);
		}

		protected virtual void callBack_Receive(IAsyncResult ar)
		{
			// 从异步state对象中获取state和socket对象
			var state = ar.AsyncState as SoMsgAcceptor;
			Socket so = state.workSocket;
			// 从客户socket读取数据
			int bytesRead = so.EndReceive(ar);
			if (bytesRead > 0)
			{
				Boolean tmp = state.appendMsg(bytesRead);
				if (!tmp)
				{
					Basic.trace("continue receiving...");
					so.BeginReceive(state.buffer, 0, state.bytesToReceive, 0,
						new AsyncCallback(callBack_Receive), state);
					return;
				}
			}
			Basic.trace("finish receiving : " + state.msgBody);
			receivedMsgHandler(state);           
		}

		protected virtual void callBack_Send(IAsyncResult ar)
		{
			Socket handler = ar.AsyncState as Socket;
			int bytesToSend = handler.EndSend(ar);
			handler.Shutdown(SocketShutdown.Both);
			handler.Dispose();
		}

		public virtual void Send(Socket cli , string message)
		{
			Basic.trace("Send "+message+" to "+cli.RemoteEndPoint);
//            byte[] msg = Encoding.UTF8.GetBytes(message);
			byte[] msg = Encoding.Default.GetBytes(message);
			cli.BeginSend(msg, 0, msg.Length, 0, new AsyncCallback(callBack_Send), cli);
		}

		public void stopListening()
		{
			Basic.trace("stop listening socket access.");
			listenStatus = false;
		}
		/// <summary> 处理收到的信息，子类需要override这个方法 </summary>
		protected abstract void receivedMsgHandler(Object state);

	}
}