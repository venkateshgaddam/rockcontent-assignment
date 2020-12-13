using RockContent.Common.Models;
using RockContent.Features.ArticleLike.Command.DataService;
using RockContent.Features.ArticleLike.Command.Validator;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RockContent.Features.ArticleLike.Command.Biz
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> AddLiketoArticle(CreateLikeModel like)
        {
            try
            {
                LikeDbModel likeDbModel = new LikeDbModel()
                                            .SetArticleId(like.ArticleId)
                                            .SetTimeStamp(DateTimeOffset.UtcNow)
                                            .SetUserId(like.UserId)
                                            .SetCurrentState(like.CurrentState);
                RequestValidator requestValidator = new RequestValidator()
                                                        .SetRequestValidator(likeDbModel)
                                                        .ValidateAnyNull();

                if (requestValidator.isValidRequest)
                {

                    likeDbModel = await articleDataService.UpdateArticleLikes(likeDbModel);
                }

                //Update the Redis
                articleDataService.UpdateRedis(likeDbModel.ArticleId, true);

                return HttpStatusCode.Created;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> UpdateArticleLike(UpdateLikeModel like)
        {
            try
            {
                LikeDbModel likeDbModel = new LikeDbModel()
                                            .SetArticleId(like.ArticleId)
                                            .SetTimeStamp(DateTimeOffset.UtcNow)
                                            .SetUserId(like.UserId)
                                            .SetCurrentState(like.CurrentState);
                likeDbModel.LikeId = like.LikeId;

                RequestValidator requestValidator = new RequestValidator()
                                                        .SetRequestValidator(likeDbModel)
                                                        .ValidateAnyNull();

                if (requestValidator.isValidRequest)
                {

                    likeDbModel = await articleDataService.UpdateArticleLikes(likeDbModel);
                }

                //Update the Redis
                articleDataService.UpdateRedis(likeDbModel.ArticleId, true);

                return HttpStatusCode.Created;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

    }
}
