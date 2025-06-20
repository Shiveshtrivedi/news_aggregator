using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto { CategoryId = c.CategoryId, Name = c.CategoryName });
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                throw new InvalidCategoryException("Category name must not be empty.");

            bool exists = await _categoryRepository.ExistsAsync(dto.CategoryName);
            if (exists)
                throw new InvalidCategoryException("Category already exists.");

            var category = new Category { CategoryName = dto.CategoryName };
            if (category.CategoryName == "")
                throw new Exception("enter string");
            await _categoryRepository.AddAsync(category);
            return new CategoryDto { CategoryId = category.CategoryId, Name = category.CategoryName };
        }

    }
}
