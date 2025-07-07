using AutoMapper;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;
using news_aggregator.shared.CustomException.CategoryException;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace news_aggregator.application.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                throw new CategoryOperationException("Failed to retrieve all categories.", ex);
            }
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.CategoryName))
                    throw new InvalidCategoryException("Category name must not be empty.");

                bool exists = await _categoryRepository.CategoryExistsAsync(dto.CategoryName);
                if (exists)
                    throw new InvalidCategoryException("Category already exists.");

                var category = new Category { CategoryName = dto.CategoryName };
                if (string.IsNullOrEmpty(category.CategoryName))
                    throw new InvalidCategoryException("Category name cannot be blank.");

                await _categoryRepository.AddAsync(category);
                return _mapper.Map<CategoryDto>(category);
            }
            catch (InvalidCategoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CategoryOperationException("Error occurred while creating category.", ex);
            }
        }

        public async Task<bool> ToggleCategoryVisibilityAsync(int categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null) return false;

                category.IsHidden = !category.IsHidden;
                await _categoryRepository.UpdateAsync(category);
                return true;
            }
            catch (Exception ex)
            {
                throw new CategoryOperationException($"Failed to toggle visibility for category ID {categoryId}.", ex);
            }
        }
    }
}
