using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/admin/keywords")]
    [Authorize(Roles = "Admin")]
    public class KeywordController : ControllerBase
    {
        private readonly IBlockedKeywordService _keywordService;

        public KeywordController(IBlockedKeywordService keywordService)
        {
            _keywordService = keywordService;
        }

        [HttpPost("addBlockedKeyword")]
        public async Task<IActionResult> AddKeyword([FromBody] string keyword)
        {
            await _keywordService.AddKeywordAsync(keyword);
            return Ok("Keyword added successfully.");
        }

        [HttpGet("listBlockedKeyword")]
        public async Task<IActionResult> GetAllKeywords()
        {
            var keywords = await _keywordService.GetAllKeywordsAsync();
            return Ok(keywords);
        }

        [HttpDelete("removeBlockedKeyword")]
        public async Task<IActionResult> RemoveKeyword([FromQuery] string keyword)
        {
            await _keywordService.RemoveKeywordAsync(keyword);
            return Ok("Keyword removed.");
        }
    }
}
