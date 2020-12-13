using StackExchange.Redis;
using System;

namespace RockContent.Common
{
    public class RedisCacheRepository : IRedisCacheRepository
    {
        private readonly RedisConnector redisConnector;

        private readonly IDatabase _database;

        public RedisCacheRepository(RedisConnector redis)
        {
            this.redisConnector = redis;

            //_database =   this.redisConnector?.GetDB;
        }

        public string GetRedisData(string redisKey)
        {
            return _database.StringGet(redisKey).ToString();

        }

        public void SetRedisData(string key, string value, TimeSpan? expiry = null)
        {
            _database.StringSet(key, value, expiry);

        }
    }
}
