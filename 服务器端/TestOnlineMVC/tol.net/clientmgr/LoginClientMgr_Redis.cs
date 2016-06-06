using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CSRedis;

namespace TestOnlineMVC.tol.net.clientmgr
{
    public class LoginClientMgr_Redis : IClientMgr
    {
        private static LoginClientMgr_Redis ins;
        public static void init() { if(ins==null) ins = new LoginClientMgr_Redis();}
        public static LoginClientMgr_Redis instance {
            get { return ins; }
        }

        protected RedisClient dbcon;
        protected const String DB_ADDR = "localhost";
        protected const String DB_PASSWORD = "tsangchiwan";

        private LoginClientMgr_Redis ()
        {
            dbcon = new RedisClient(DB_ADDR);
            dbcon.Auth(DB_PASSWORD);
        }

        public void Login(string num, string sid)
        {
            var cli = dbcon.HGet("Client:Login", num);
            if (cli != null)
            {
                dbcon.HSet("Client:Ban", cli, num);
            }
        }

        public void Logout(string sid)
        {
            throw new NotImplementedException();
        }

        public void Delete(string sid)
        {
            throw new NotImplementedException();
        }

        public bool isLogin(string sid)
        {
            throw new NotImplementedException();
        }

        public bool isBan(string sid)
        {
            return dbcon.HExists("Client:Ban", sid);
        }

        public void heartBeat(string sid)
        {
            throw new NotImplementedException();
        }

        public void startHeartBeatTest()
        {
            throw new NotImplementedException();
        }

        public void stopHeartBeatTest()
        {
            throw new NotImplementedException();
        }

        public void searchByNumber(String num)
        {
            dbcon.HGet("Client:Login",num);
        }

        public void searchBySid(String sid)
        {
            
        }
    }
}