using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application;
using news_aggregator.application.Interfaces.Services;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly INewsQueryService _newsQueryService;

        public NewsController(INewsService newsService, INewsQueryService newsQueryService)
        {
            _newsService = newsService;
            _newsQueryService = newsQueryService;
        }

        //[Authorize(Roles = "Admin")]

        //[HttpGet("getNewsByExternalApi")]
        //public async Task<IActionResult> GetFromExternal()
        //{
        //    var articles = await _newsService.FetchAndSaveExternalNewsAsync();
        //    return Ok(articles);
        //}

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
            if (startDate > endDate)
                return BadRequest("Start date must be before end date.");

            var articles = await _newsQueryService.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);
            return Ok(articles);
        }

    }
}
