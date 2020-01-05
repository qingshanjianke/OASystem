using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MemcacheHelper
    {
        private static readonly MemcachedClient mc = null;
        
        static MemcacheHelper()
        {
            string[] serverlist = { "127.0.0.1:11211" };

            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();

            // 获得客户端实例
            mc = new MemcachedClient();
            mc.EnableCompression = false;
        }

        public static bool Set(string key,object value) {
            return mc.Set(key, value);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dateTime">过期时间</param>
        /// <returns></returns>
        public static bool Set(string key, object value,DateTime dateTime)
        {
            return mc.Set(key, value, dateTime);
        }

        public static object Get(string key)
        {
                return mc.Get(key);
        }

        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))   //判断有无该Key
            {
                return mc.Delete(key);
            }
            return false;
        }
    }
}