using RockContent.Common.Models;
using System.Net;
using System.Threading.Tasks;

/// <summary>
///     Business Layer where the BusinessLogic implementations takes place
/// </summary>
namespace RockContent.Features.ArticleLike.Command.Biz
{
    public interface IArticleBiz
    {
        /// <summary>
        ///     Fetch the Total Likes of the Particluar Article based on the ArticleID
        /// </summary>
        /// <returns></returns>
        Task<HttpStatusCode> AddLiketoArticle(CreateLikeModel like);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        Task<HttpStatusCode> UpdateArticleLike(UpdateLikeModel like);

    }
}
