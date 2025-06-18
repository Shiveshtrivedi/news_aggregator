using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            await _savedArticleService.SaveArticleAsync(userId, articleId);
            return Ok(new { Message = "Article saved successfully." });
        }

        [HttpGet("{userId}/getArticleFromUserId")]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetSavedArticles(int userId)
        {
            var articles = await _savedArticleService.GetSavedArticlesByUserIdAsync(userId);
            return Ok(articles);
        }

        [HttpDelete("{userId}/{articleId}/unsaveArticle")]
        public async Task<IActionResult> DeleteSavedArticle(int userId, int articleId)
        {
            await _savedArticleService.DeleteSavedArticleAsync(userId, articleId);
            return Ok(new { Message = "Saved article deleted successfully." });
        }

    }
}
