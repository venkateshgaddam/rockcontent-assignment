using RockContent.Features.ArticleLike.Query.DataService;
using System;
using System.Threading.Tasks;

namespace RockContent.Features.ArticleLike.Query.Biz
{
    public class ArticleBiz : IArticleBiz
    {
        #region Properties

        private readonly IArticleDataService articleDataService;

        #endregion

        #region Constructor
        
        public ArticleBiz(IArticleDataService articleDataService)
        {
            this.articleDataService = articleDataService;
        }
        
        #endregion

        #region Methods

        public async Task<long> GetTotalLikesByArticle(string articleId)
        {
            try
            {
                var result = await articleDataService.GetArticleLikesCountAsync(articleId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
           
        }
        
        #endregion

    }
}
