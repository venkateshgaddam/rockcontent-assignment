using RockContent.Common.Models;
using System;
using System.Threading.Tasks;

namespace RockContent.Features.ArticleLike.Command.DataService
{
    public interface IArticleDataService
    {
        ///// <summary>
        /////     Fetch the Number of Likes for a particular Article
        ///// </summary>
        ///// <param name="articleId"></param>
        ///// <returns></returns>
        //Task<long> GetArticleLikesCountAsync(string articleId); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        Task<LikeDbModel> AddArticleLike(LikeDbModel like);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        Task<LikeDbModel> UpdateArticleLikes(LikeDbModel like);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        void UpdateRedis(Guid articleId, bool currentState);

    }
}
