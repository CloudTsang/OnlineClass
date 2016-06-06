using System;
using TestOnlineMVC.tol.socketserver.connection.asyncstate;

namespace TestOnlineMVC.tol.socketserver.connection
{
    public class SocketListener : SocketListenner_Template
    {
        protected StudentDataProcessor _student=new StudentDataProcessor();
        protected TeacherDataProcessor _teacher=new TeacherDataProcessor();

        public SocketListener(int len = 60) : base(len){ }

        protected override void receivedMsgHandler(Object state)
        {
            var tmp = state as ISocketMsg;
            String result=String.Empty;
            switch ( tmp.clientType )
            {
                case 0:
                    result=_student.dataProcess(tmp);
                    break;
                case 1:
                    result=_teacher.dataProcess(tmp);
                    break;
            }
            Send((tmp as StateObject).workSocket, result);
        }
    }
}