using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.shared.CustomException;
using news_application.Models;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SavedArticleController : ControllerBase
    {
        private readonly ISavedArticleService _savedArticleService;

        public SavedArticleController(ISavedArticleService savedArticleService)
        {
            _savedArticleService = savedArticleService;
        }

        [HttpPost("{userId}/{articleId}/saveArticle")]
        public async Task<IActionResult> SaveArticle(int userId, int articleId)
        {
            try
            {
                await _savedArticleService.SaveArticleAsync(userId, articleId);
                return Ok(new { Message = "Article saved successfully." });
            }
            catch (SaveArticleFailedException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpGet("{userId}/getArticleFromUserId")]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetSavedArticles(int userId)
        {
            try
            {
                var articles = await _savedArticleService.GetSavedArticlesByUserIdAsync(userId);
                return Ok(articles);
            }
            catch (GetSavedArticlesFailedException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpDelete("{userId}/{articleId}/unsaveArticle")]
        public async Task<IActionResult> DeleteSavedArticle(int userId, int articleId)
        {
            try
            {
                await _savedArticleService.DeleteSavedArticleAsync(userId, articleId);
                return Ok(new { Message = "Saved article deleted successfully." });
            }
            catch (DeleteSavedArticleFailedException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}
