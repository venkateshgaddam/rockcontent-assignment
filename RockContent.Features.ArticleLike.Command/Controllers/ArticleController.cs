using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RockContent.Common.Models;
using RockContent.Features.ArticleLike.Command.Biz;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RockContent.Features.ArticleLike.Command.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        #region Properties

        private readonly IArticleBiz articleBiz;

        #endregion

        #region Constructor

        public ArticleController(IArticleBiz articleBiz)
        {
            this.articleBiz = articleBiz;
        }
        #endregion

        #region Methods

        [HttpPost("AddLiketoArticle")]
        public async Task<IActionResult> AddLike([FromBody] CreateLikeModel likeModel)
        {
            try
            {
                var data = await articleBiz.AddLiketoArticle(likeModel);

                var result = new JsonResult(data)
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Created)
                };
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPatch("UpdateLiketoArticle")]
        public async Task<IActionResult> UpdateLikeForArticle([FromBody] UpdateLikeModel likeModel)
        {
            try
            {
                var data = await articleBiz.UpdateArticleLike(likeModel);

                var result = new JsonResult(data)
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.Created)
                };
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
