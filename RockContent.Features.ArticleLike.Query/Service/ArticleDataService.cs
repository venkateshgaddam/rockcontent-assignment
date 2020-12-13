using RockContent.Common;
using RockContent.Common.DAL;
using RockContent.Common.Models;
using System;
using System.Threading.Tasks;

namespace RockContent.Features.ArticleLike.Query.DataService
{
    public class ArticleDataService : IArticleDataService
    {
        #region Properties

        private IGenericRepository<Activity> genericRepository;

        private readonly IRedisCacheRepository redisCacheRepository;

        #endregion

        #region Constructor

        public ArticleDataService(IGenericRepository<Activity> genericRepository, IRedisCacheRepository redisCacheRepository)
        {
            this.genericRepository = genericRepository;
            this.redisCacheRepository = redisCacheRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fetch the Total LikesCount for the Article based on the ArticleId
        /// </summary>
        /// <param name="articleId"> The Article Id </param>
        /// <returns> The Total Likes Count </returns>
        public async Task<long> GetArticleLikesCountAsync(string articleId)
        {
            try
            {
                var redisData = this.redisCacheRepository.GetRedisData(string.Format(GlobalConstants.LIKES_COUNT_REDIS_KEY, articleId));
                return long.Parse(redisData);
            }
            catch (Exception)
            {
                return (await genericRepository.GetListAsync("ArticleId", articleId)).Count;
            }
        }

        #endregion

    }
}
