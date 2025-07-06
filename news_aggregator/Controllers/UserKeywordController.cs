using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/user/keywords")]
    [Authorize]
    public class UserKeywordController : ControllerBase
    {
        private readonly IUserKeywordService _userKeywordService;

        public UserKeywordController(IUserKeywordService userKeywordService)
        {
            _userKeywordService = userKeywordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetKeywords()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")!.Value);
                var keywords = await _userKeywordService.GetKeywordsAsync(userId);
                return Ok(keywords);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred while fetching keywords." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetKeywords([FromBody] List<string> keywords)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")!.Value);
                await _userKeywordService.SetKeywordsAsync(userId, keywords);
                return Ok("Keywords updated.");
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred while setting keywords." });
            }
        }
    }
}
