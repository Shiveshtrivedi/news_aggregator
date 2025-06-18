using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_application.Models;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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

        [HttpPost]
        [Route("addExternalSourceApi")]
        public async Task<IActionResult> AddExternalSource([FromBody] CreateExternalSourceDto dto)
        {
            await _externalSourceService.AddExternalSourceApi(dto);
            return Ok("External source added successfully.");
        }

        [HttpPatch("{externalSourceId}")]
        public async Task<IActionResult> UpdatePartial(int externalSourceId, [FromBody] UpdateExternalSourceDto dto)
        {
            var result = await _externalSourceService.UpdatePartialAsync(externalSourceId, dto);
            if (!result)
                return NotFound("External Source not found.");

            return Ok("Updated successfully.");
        }


    }
}
