using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("getAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
            return Ok(await _categoryService.GetAllAsync());

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            try
            {
               return Ok(await _categoryService.CreateAsync(dto));
            }
            catch (InvalidCategoryException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

    }
}
