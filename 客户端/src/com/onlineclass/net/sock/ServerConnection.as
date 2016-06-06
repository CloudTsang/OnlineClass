package com.onlineclass.net.sock
{
	import com.onlineclass.info.LoginInfo;
	import com.onlineclass.net.IConnection;
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.net.Socket;
	
	/**与服务器进行Socket通信**/
	public class ServerConnection extends EventDispatcher implements IConnection
	{
		protected var _socket:Socket;
		protected var _switch:Boolean;
		public function ServerConnection()
		{
			_socket = new Socket();
			_socket.addEventListener(Event.CONNECT , onConnectHandler);
			_socket.addEventListener(IOErrorEvent.IO_ERROR , errHandler);
			_socket.addEventListener(ProgressEvent.SOCKET_DATA , getData);
			//			_socket.addEventListener(Event.CLOSE , connectionClosed);
			_switch=true;
		}
		/**连接处理**/
		protected function onConnectHandler(e:Event):void{
			trace("已连接到服务器");
			_socket.addEventListener(Event.CLOSE , connectionClosed);
			_socket.removeEventListener(Event.CONNECT , onConnectHandler);
		}
		/**获得数据处理*/
		protected function getData(e:ProgressEvent):void{
			throw new Error("This function should be overrided!");
		}
		/**连接断开处理**/
		protected function connectionClosed(e:Event):void{
			trace("Socket连接已断开");
			_socket.removeEventListener(Event.CLOSE , connectionClosed);
			//			_socket.removeEventListener(ProgressEvent.SOCKET_DATA , getData);
			//			_socket.removeEventListener(IOErrorEvent.IO_ERROR , errHandler);
		}
		/**连接出错处理**/
		protected function errHandler(e:IOErrorEvent):void{
			trace("连接出错："+e.text);
		}
		public function set connectStatus(v:Boolean):void{
			if(v) {
				if(_socket.connected) return ;// throw new Error("Socket has already connected!");
				_socket.connect( Basic.IP_SO , Basic.PORT );
				_socket.addEventListener(Event.CONNECT , onConnectHandler);
				//				_socket.addEventListener(IOErrorEvent.IO_ERROR , errHandler);
				//				_socket.addEventListener(ProgressEvent.SOCKET_DATA , getData);
			}
			else{
				if(!_socket.connected) return ; //throw new Error("Socket has already disconnected!");
				_socket.close();
			}
		}
		public function Send(message:Object):void{
			trace("向服务器发出信息："+message);
			//			_socket.writeUTFBytes( message as String );
			if(!_switch)return;
			_switch=false;
			_socket.writeMultiByte(( message as String ) , SocketMessage.GB);
			_socket.flush();
		}
	}
}