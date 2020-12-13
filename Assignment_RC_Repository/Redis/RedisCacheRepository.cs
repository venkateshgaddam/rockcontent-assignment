using StackExchange.Redis;
using System;

namespace RockContent.Common
{
    public class RedisCacheRepository : IRedisCacheRepository
    {
        private readonly RedisConnector redisConnector;

        private readonly IDatabase _database;
        
        public RedisCacheRepository(RedisConnector redisConnector)
        {
            this.redisConnector = redisConnector;
            _database = this.redisConnector?.GetDB;
        }

        public string GetRedisData(string redisKey)
        {
            try
            {
                return _database.StringGet(redisKey).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SetRedisData(string key, string value, TimeSpan? expiry = null)
        {
            try
            {
                _database.StringSet(key, value, expiry);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
