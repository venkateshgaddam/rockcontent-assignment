using Npgsql;
using RockContent.Common;
using RockContent.Common.DAL;
using RockContent.Common.Models;
using System;
using System.Threading.Tasks;

namespace RockContent.Features.ArticleLike.Command.DataService
{
    public class ArticleDataService : IArticleDataService
    {
        #region Properties

        private IGenericRepository<LikeDbModel> genericRepository;

        private readonly IRedisCacheRepository redisCacheRepository;

        #endregion

        #region Constructor

        public ArticleDataService(IGenericRepository<LikeDbModel> genericRepository, IRedisCacheRepository redisCacheRepository)
        {
            this.genericRepository = genericRepository;
            this.redisCacheRepository = redisCacheRepository;
        }

        #endregion

        #region Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        public async Task<LikeDbModel> AddArticleLike(LikeDbModel like)
        {
            try
            {
                var result = await genericRepository.AddAsync(like);
                return result;
            }
            catch (NpgsqlException)
            {
                //Logging Mechanism
                throw;
            }
        }

        public async Task<LikeDbModel> UpdateArticleLikes(LikeDbModel like)
        {
            try
            {
                var result = await genericRepository.UpdateAsync(like);
                return result;
            }
            catch (NpgsqlException)
            {
                //Logging Mechanism
                throw;
            }
        }

        public void UpdateRedis(Guid articleId, bool currentState)
        {
            if (currentState)
            {
                var prevLikesCount = Convert.ToInt32(redisCacheRepository.GetRedisData(string.Format(GlobalConstants.LIKES_COUNT_REDIS_KEY, articleId.ToString())));
                redisCacheRepository.SetRedisData(string.Format(GlobalConstants.LIKES_COUNT_REDIS_KEY, articleId.ToString()), (++prevLikesCount).ToString(), expiry: TimeSpan.FromDays(365));
            }
            else
            {
                var prevLikesCount = Convert.ToInt32(redisCacheRepository.GetRedisData(string.Format(GlobalConstants.LIKES_COUNT_REDIS_KEY, articleId.ToString())));
                redisCacheRepository.SetRedisData(string.Format(GlobalConstants.LIKES_COUNT_REDIS_KEY, articleId.ToString()), (--prevLikesCount).ToString(), expiry: TimeSpan.FromDays(365));
            }
        }

        #endregion

    }
}
