using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;
//using news_aggregator.infrastructure.Jobs;
using news_application.Models;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalSourceController : ControllerBase
    {
        private readonly IExternalSourceService _externalSourceService;

        public ExternalSourceController(IExternalSourceService externalSourceService)
        {
            _externalSourceService = externalSourceService;
        }

        [HttpGet("getAllExternalSources")]
        public async Task<IActionResult> GetAllExternalSources()
        {
            try
            {
                var externalSources = await _externalSourceService.GetAllSourcesAsync();

                return Ok(externalSources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{externalSourceId}/getExternalSoureById")]
        public async Task<IActionResult> GetExternalSourceById(int externalSourceId)
        {
            try
            {
                var externalSource = await _externalSourceService.GetSourceByIdAsync(externalSourceId);
                return Ok(externalSource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addExternalSource")]
        public async Task<IActionResult> addExternalSource([FromBody] ExternalSource externalSource)
        {
            try
            {
                await _externalSourceService.AddSourceAsync(externalSource);
                return Ok("External Sources added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{externalSourceId}/updateExternalSource")]
        public async Task<IActionResult> UpdateExternalSource(int externalSourceId, [FromBody] ExternalSource source)
        {
            try
            {
                if (externalSourceId != source.ExternalSourceId)
                {
                    return BadRequest("Enter Correct Id");
                }

                await _externalSourceService.UpdateSourceAsync(source);
                return Ok("Source Update successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("fetch-external")]
        //public async Task<IActionResult> FetchFromExternalApi([FromServices] NewsFetcherJob job)
        //{
        //    await job.FetchAndStoreAsync();
        //    return Ok("News fetched and stored.");
        //}

    }
}
