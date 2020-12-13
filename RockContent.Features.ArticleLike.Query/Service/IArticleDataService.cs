using System.Collections.Generic;
using System.Threading.Tasks;

namespace RockContent.Features.ArticleLike.Query.DataService
{
    public interface IArticleDataService
    {
        /// <summary>
        ///     Fetch the Number of Likes for a particular Article
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<long> GetArticleLikesCountAsync(string articleId); 
    }
}
