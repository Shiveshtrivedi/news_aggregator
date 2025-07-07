using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.shared.CustomException.ExternalSource;
using news_aggregator.shared.CustomException.NewsArticle;
using System.Security.Claims;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly INewsQueryService _newsQueryService;
        private readonly INewsInteractionService _newsInteractionService;
        private readonly IReportArticleService _reportArticleService;
        private readonly IRecommendationService _recommendationService;

        public NewsController(INewsService newsService, INewsQueryService newsQueryService, INewsInteractionService newsInteractionService, IReportArticleService reportArticleService, IRecommendationService recommendationService)
        {
            _newsService = newsService;
            _newsQueryService = newsQueryService;
            _newsInteractionService = newsInteractionService;
            _reportArticleService = reportArticleService;
            _recommendationService = recommendationService;
        }

        //[Authorize(Roles = "Admin")]

        [HttpGet("getNewsByExternalApi")]
        public async Task<IActionResult> GetFromExternal()
        {
            try
            {
                var articles = await _newsService.FetchAndSaveExternalNewsAsync();
                return Ok(articles);
            }
            catch (ExternalSourceNotFoundException ex)
            {
                return StatusCode(503, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Failed to fetch external news." });
            }
        }

        [HttpGet("searchNews")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var articles = await _newsQueryService.SearchNewsByTitleAsync(title, startDate, endDate);
                return Ok(articles);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while searching news." });
            }
        }

        [HttpGet("getNewsByDateRange")]
        public async Task<IActionResult> GetNewsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            try
            {
                var articles = await _newsQueryService.GetNewsByDateRangeAsync(startDate, endDate);
                return Ok(articles);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving news." });
            }
        }

        [HttpGet("getNewsByCategoryAndDateRange")]
        public async Task<IActionResult> GetNewsByCategoryAndDateRange([FromQuery] string category, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest("Start date must be before end date.");

                var userIdClaim = User.FindFirst("UserId");

                if (userIdClaim == null)
                    return Unauthorized("User ID claim missing.");

                if (!int.TryParse(userIdClaim.Value, out int userId))
                    return Unauthorized("Invalid user ID.");

                var articles = await _newsQueryService.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate, userId);
                return Ok(articles);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPost("{articleId}/like")]
        public async Task<IActionResult> ToggleLike(int articleId)
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
                return Unauthorized("User ID claim missing.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID.");

            try
            {
                await _newsInteractionService.ToggleLikeAsync(articleId, userId);
                return Ok(new { message = "Like toggled successfully." });
            }
            catch (NewsArticleNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UserArticleInteractionException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while toggling like." });
            }
        }

        [Authorize]
        [HttpPost("{articleId}/dislike")]
        public async Task<IActionResult> ToggleDislike(int articleId)
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
                return Unauthorized("User ID claim missing.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID.");

            try
            {
                await _newsInteractionService.ToggleDislikeAsync(articleId, userId);
                return Ok(new { message = "Dislike toggled successfully." });
            }
            catch (NewsArticleNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UserArticleInteractionException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while toggling dislike." });
            }
        }


        [HttpPost("{articleId}/report")]
        public async Task<IActionResult> ReportArticle(int articleId, [FromBody] ReportRequestDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId");
                if (userIdClaim == null)
                    return Unauthorized("User ID claim missing.");

                if (!int.TryParse(userIdClaim.Value, out int userId))
                    return Unauthorized("Invalid user ID.");

                var success = await _reportArticleService.ReportArticleAsync(articleId, userId, dto.Message);
                if (!success)
                    return BadRequest("You have already reported this article.");

                return Ok("Article reported successfully.");
            }
            catch (UserReportCheckException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (ReportAddException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (ReportCountFetchException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (ReportProcessException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }


        [HttpGet("personalized")]
        public async Task<IActionResult> GetPersonalizedArticles()
        {
            int userId = int.Parse(User.FindFirst("userId")?.Value!);
            var result = await _recommendationService.GetPersonalizedArticlesAsync(userId);
            return Ok(result);
        }

    }
}
