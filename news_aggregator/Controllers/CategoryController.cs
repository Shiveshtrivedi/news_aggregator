using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;

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
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            return Ok(await _categoryService.CreateAsync(dto));
        }

    }
}
