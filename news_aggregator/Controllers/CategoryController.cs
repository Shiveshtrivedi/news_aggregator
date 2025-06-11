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

        [HttpGet("{categoryId}/getCategoryById")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _categoryService.GetByIdAsync(categoryId);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            return Ok(await _categoryService.CreateAsync(dto));
        }


        [HttpPut("{categoryId}/updateCategory")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CreateCategoryDto dto)
        {
            return Ok(await _categoryService.UpdateAsync(categoryId, dto));
        }


        [HttpDelete("{categoryId}/deleteCategory")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            return Ok(await _categoryService.DeleteAsync(categoryId));
        }
            
    }
}
