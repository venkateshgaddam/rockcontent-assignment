using System.Threading.Tasks;

namespace RockContent.Features.ArticleLike.Query.Biz
{
    public interface IArticleBiz
    {
        /// <summary>
        ///     Fetch the Total Likes of the Particluar Article based on the ArticleID
        /// </summary>
        /// <returns></returns>
        Task<long> GetTotalLikesByArticle(string articleId);

    }
}
