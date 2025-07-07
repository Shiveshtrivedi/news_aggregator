using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException.CategoryException;
using System;
using System.Threading.Tasks;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            catch (CategoryOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving categories." });
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
                return BadRequest(new { message = ex.Message });
            }
            catch (CategoryOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{categoryId}/categoryVisibilityToggle")]
        public async Task<IActionResult> ToggleVisibility(int categoryId)
        {
            try
            {
                var success = await _categoryService.ToggleCategoryVisibilityAsync(categoryId);
                if (!success)
                    return NotFound(new { message = "Category not found." });

                return Ok(new { message = "Visibility toggled successfully." });
            }
            catch (CategoryOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
