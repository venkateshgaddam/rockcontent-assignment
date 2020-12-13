using System;
using System.Collections.Generic;
using System.Text;

namespace RockContent.Common.Redis
{

    // <summary>
    ///     This class present require configuration for dealing with Redis cache.
        /// </summary>
    public class RedisConfig
    {
        public string HostName { get; set; }
        public string Port { get; set; }
        public string Key { get; set; }
        public string SSL { get; set; } = "True";
    }
}
