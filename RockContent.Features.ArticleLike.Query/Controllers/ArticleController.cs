using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RockContent.Features.ArticleLike.Query.Biz;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RockContent.Features.ArticleLike.Query.Controllers
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

        [HttpGet]
        [Route("GetArticleLikes/{ArticleId}")]
        public async Task<IActionResult> GetLikesByArticleId(string ArticleId)
        {
            try
            {
                var totalLikesCount = await articleBiz.GetTotalLikesByArticle(ArticleId);
                var result = new JsonResult(totalLikesCount)
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.OK)
                };
                return result;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error in Processing the Request", ex.Message);
                return BadRequest();
            }
        }
        
        #endregion

    }
}
