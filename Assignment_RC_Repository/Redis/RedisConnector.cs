using RockContent.Common.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace RockContent.Common
{
    public class RedisConnector
    {
        private readonly RedisConfig _redisConfig;
        private Lazy<ConnectionMultiplexer> _lazyConnection;

        public RedisConnector(RedisConfig redisConfig)
        {
            this._redisConfig = redisConfig;
            this._lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(GetConnectionString());
            });
        }

        public ConnectionMultiplexer connection => _lazyConnection.Value;

        public IDatabase GetDB => connection.GetDatabase();

        public IServer GetServer => connection.GetServer(_redisConfig.HostName, Convert.ToInt32(_redisConfig.Port));

        private string GetConnectionString()
        {
            var redisConfig = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                Ssl = Convert.ToBoolean(this._redisConfig.SSL),
                ConnectRetry = 3,
                ConnectTimeout = 1000 * 30,
                AsyncTimeout = 1000 * 3,
                SyncTimeout = 1000 * 3,
                ReconnectRetryPolicy = new ExponentialRetry(5000, 30000),
                DefaultDatabase = 0,
                AllowAdmin = true,
                CommandMap = CommandMap.Create(new HashSet<string> { "INFO", "ECHO" }, available: false),
                EndPoints =
                {
                    {this._redisConfig.HostName,Convert.ToInt32(this._redisConfig.Port) }
                },
                Password = this._redisConfig.Key
            };
            return redisConfig.ToString();
        }

    }

}
