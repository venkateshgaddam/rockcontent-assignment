namespace RockContent.Common
{
    public static class GlobalConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string LIKES_COUNT_REDIS_KEY = "RockContent|Article|{0}|Likes|Count";

        /// <summary>
        /// 
        /// </summary>
        public const string LIKE_DATA_REDIS_KEY = "RockContent|Like|{0}";
        
        /// <summary>
        /// 
        /// </summary>
        public const string REDIS_CONFIG_KEY = "AppSettings:RedisConfig";

        /// <summary>
        /// 
        /// </summary>
        public const string DATABASE_CONFIG_KEY = "ConnectionStrings:DatabaseConnectionString";

        /// <summary>
        /// 
        /// </summary>
        public const string SWAGGER_TITLE = "RockContent-Assignment";

        /// <summary>
        /// 
        /// </summary>
        public const string SWAGGER_APP_NAME = "LikeFeature- {0} API";

        /// <summary>
        /// 
        /// </summary>
        public const string SWAGGER_DESCRIPTION = "Like Feature Command";

        /// <summary>
        /// 
        /// </summary>
        public const string SWAGGER_URL = "v1/swagger.json";
    }
}
