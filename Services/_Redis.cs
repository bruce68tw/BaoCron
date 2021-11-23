using Base.Services;
using CSRedis;

namespace BaoCron.Services
{
    /// <summary>
    /// 將 Cache class 包裝成靜態類別
    /// </summary>
    public static class _Redis
    {
        private static CSRedisClient _redis;

        //constructor
        static _Redis()
        {
            _redis = new CSRedisClient(_Fun.Config.Redis);
        }

        /// <summary>
        /// delete all keys in current database index
        /// </summary>
        public static void DeleteAll()
        {
            _redis.Del(_redis.Keys("*"));
        }

        /*
        /// <summary>
        /// get cache of query result
        /// </summary>
        /// <param name="stCache"></param>
        /// <returns>0(no cache server), -1(no cache rows), 1(get cache rows)</returns>
        public static JArray GetQuery(StCache stCache)
        {
            return new Cache().GetQuery(stCache);
        }

        /// <summary>
        /// get group row of cache
        /// </summary>
        /// <param name="cache"></param>
        /// <returns>0(no cache server), -1(not found), 1(found)</returns>
        public static JArray GetGroup(StCache stCache)
        {
            return new Cache().GetGroup(stCache);
        }

        public JObject GetRow(StCache stCache, int sn)
        {
            return new Cache().GetRow(stCache, sn);
        }

        public void DeleteKeys(List<StDeleteCache> rows, string db="")
        {
            new Cache().DeleteKeys(rows, db);
        }

        public void DeleteKey(string table, List<string> fids, string db = "")
        {
            new Cache().DeleteKey(table, fids, db, true);
        }
        */
    }
}
