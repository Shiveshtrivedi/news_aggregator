using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application;
using news_aggregator.application.Interfaces.Services;
using System.Security.Claims;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly INewsQueryService _newsQueryService;
        private readonly INewsInteractionService _newsInteractionService;

        public NewsController(INewsService newsService, INewsQueryService newsQueryService, INewsInteractionService newsInteractionService)
        {
            _newsService = newsService;
            _newsQueryService = newsQueryService;
            _newsInteractionService = newsInteractionService;
        }

        //[Authorize(Roles = "Admin")]

        [HttpGet("getNewsByExternalApi")]
        public async Task<IActionResult> GetFromExternal()
        {
            var articles = await _newsService.FetchAndSaveExternalNewsAsync();
            return Ok(articles);
        }

        [HttpGet("searchNews")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var articles = await _newsQueryService.SearchNewsByTitleAsync(title, startDate, endDate);
            return Ok(articles);
        }

        [HttpGet("getNewsByDateRange")]
        public async Task<IActionResult> GetNewsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var articles = await _newsQueryService.GetNewsByDateRangeAsync(startDate, endDate);
            return Ok(articles);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("like/{articleId}")]
        public async Task<IActionResult> ToggleLike(int articleId)
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
                return Unauthorized("User ID claim missing.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID.");

            await _newsInteractionService.ToggleLikeAsync(articleId, userId);
            return Ok(new { message = "Like toggled successfully." });
        }

        [Authorize]
        [HttpPost("dislike/{articleId}")]
        public async Task<IActionResult> ToggleDislike(int articleId)
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
                return Unauthorized("User ID claim missing.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID.");

            await _newsInteractionService.ToggleDislikeAsync(articleId, userId);
            return Ok(new { message = "Dislike toggled successfully." });
        }


    }
}
