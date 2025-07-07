using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using news_aggregator.shared.CustomException;
using news_aggregator.shared.CustomException.CategoryException;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            try
            {
                var config = await _notificationConfigService.GetOrCreateForUserAsync(userId);
                return Ok(config);
            }
            catch (NotificationRepositoryOperationException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleCategory(int userId, string category, bool enable)
        {
            try
            {
                await _notificationConfigService.ToggleCategoryAsync(userId, category, enable);
                return Ok("Category setting updated successfully.");
            }
            catch (CategoryNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (NotificationRepositoryOperationException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("keywords")]
        public async Task<IActionResult> SetKeywords(int userId, [FromBody] List<string> keywords)
        {
            try
            {
                await _userKeywordService.SetKeywordsAsync(userId, keywords);
                return Ok("Keywords updated.");
            }
            catch (NotificationRepositoryOperationException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }
    }
}
