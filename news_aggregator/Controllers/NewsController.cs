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

        [HttpGet("externalApi")]
        public async Task<IActionResult> GetFromExternal()
        {
            var articles = await _newsService.FetchAndSaveExternalNewsAsync();
            return Ok(articles);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var news = await _newsQueryService.GetAllNewsAsync();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var article = await _newsQueryService.GetNewsByIdAsync(id);
            return article != null ? Ok(article) : NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title)
        {
            var articles = await _newsQueryService.SearchNewsByTitleAsync(title);
            return Ok(articles);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var articles = await _newsQueryService.GetNewsByCategoryAsync(category);
            return Ok(articles);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetNewsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var articles = await _newsQueryService.GetNewsByDateRangeAsync(startDate, endDate);
            return Ok(articles);
        }

    }
}
