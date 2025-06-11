using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationConfigController : ControllerBase 
    {
        private readonly INotificationConfigService _notificationConfigService;
        private readonly IUserKeywordService _userKeywordService;

        public NotificationConfigController(INotificationConfigService notificationConfigService, IUserKeywordService userKeywordService)
        {
            _notificationConfigService = notificationConfigService;
            _userKeywordService = userKeywordService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetConfig(int userId)
        {
            var config = await _notificationConfigService.GetOrCreateForUserAsync(userId);
            return Ok(config);
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleCategory(int userId, string category, bool enable)
        {
            var config = await _notificationConfigService.GetOrCreateForUserAsync(userId);

            switch (category.ToLower())
            {
                case "business": config.BusinessEnabled = enable; break;
                case "entertainment": config.EntertainmentEnabled = enable; break;
                case "sports": config.SportsEnabled = enable; break;
                case "technology": config.TechnologyEnabled = enable; break;
                case "keywords": config.KeywordsEnabled = enable; break;
                default: return BadRequest("Invalid category");
            }

            await _notificationConfigService.UpdateConfigAsync(config);
            return Ok("Updated successfully.");
        }

        [HttpPost("keywords")]
        public async Task<IActionResult> SetKeywords(int userId, [FromBody] List<string> keywords)
        {
            await _userKeywordService.SetKeywordsAsync(userId, keywords);
            return Ok("Keywords updated.");
        }

        [HttpGet("keywords/{userId}")]
        public async Task<IActionResult> GetKeywords(int userId)
        {
            var keywords = await _userKeywordService.GetKeywordsAsync(userId);
            return Ok(keywords);
        }

    }
}
