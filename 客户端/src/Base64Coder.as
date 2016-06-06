package
{
	import flash.utils.ByteArray;
	import mx.utils.Base64Decoder;
	import mx.utils.Base64Encoder;
	/**Base64编、解码*/
	public class Base64Coder
	{
		public static var instance:Base64Coder = new Base64Coder();
		protected var _encoder64:Base64Encoder;
		protected var _decoder64:Base64Decoder;
		public function Base64Coder()
		{
			_encoder64 = new Base64Encoder;
			_decoder64 = new Base64Decoder;
		}
		public function Encode(str:String):String{
			_encoder64.encode(str);
			return _encoder64.toString();
		}
		public function EncodeUTF8(str:String):String{
			_encoder64.encodeUTFBytes(str);
			return _encoder64.toString();
		}
		public function Decode(str:String):String{
			_decoder64.decode(str);
			var bytes:ByteArray = _decoder64.toByteArray();
			bytes.position=0;
			var ret:ByteArray = new ByteArray();
			bytes.readBytes( ret , 0 , bytes.length );
			return String(ret);
		}
	}
}