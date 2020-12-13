using System;

namespace RockContent.Common
{
    public interface IRedisCacheRepository
    {
        string GetRedisData(string redisKey);

        void SetRedisData(string key, string value, TimeSpan? expiry = null);
    }
}
