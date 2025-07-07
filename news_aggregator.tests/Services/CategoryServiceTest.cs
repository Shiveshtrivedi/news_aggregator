using AutoMapper;
using Moq;
using news_aggregator.application.Category;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException.CategoryException;
using news_aggregator.tests.Helpers;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Services
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CategoryService _categoryService;

        public CategoryServiceTest()
        {
            _categoryService = new CategoryService(_categoryRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_ReturnsMappedCategoryDtos()
        {
            var categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "sports" },
                new Category { CategoryId = 2, CategoryName = "technology" }
            };

            var expectedDtos = CategoryMockDataGenerator.GetTestCategoryDtoList();

            _categoryRepoMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<CategoryDto>>(categories))
                .Returns(expectedDtos);

            var result = await _categoryService.GetAllAsync();

            Assert.Equal(expectedDtos.Count, new List<CategoryDto>(result).Count);
        }

        [Fact]
        public async Task CreateAsync_WithValidCategory_ReturnsCategoryDto()
        {
            var createDto = CategoryMockDataGenerator.GetCreateCategoryDto("general");

            _categoryRepoMock.Setup(reposiotry => reposiotry.CategoryExistsAsync(createDto.CategoryName))
                .ReturnsAsync(false);

            var category = new Category { CategoryId = 5, CategoryName = createDto.CategoryName };

            _categoryRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(mapper => mapper.Map<CategoryDto>(It.IsAny<Category>()))
                .Returns(new CategoryDto { CategoryId = 5, Name = createDto.CategoryName });

            var result = await _categoryService.CreateAsync(createDto);

            Assert.Equal("general", result.Name);
        }

        [Fact]
        public async Task CreateAsync_WithEmptyName_ThrowsInvalidCategoryException()
        {
            var dto = new CreateCategoryDto { CategoryName = " " };

            await Assert.ThrowsAsync<InvalidCategoryException>(() => _categoryService.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_WhenCategoryAlreadyExists_ThrowsInvalidCategoryException()
        {
            var dto = CategoryMockDataGenerator.GetCreateCategoryDto("duplicate");

            _categoryRepoMock.Setup(repo => repo.CategoryExistsAsync(dto.CategoryName))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidCategoryException>(() => _categoryService.CreateAsync(dto));
        }

        [Fact]
        public async Task ToggleCategoryVisibilityAsync_WithValidId_TogglesIsHidden()
        {
            var category = new Category { CategoryId = 1, CategoryName = "tech", IsHidden = false };

            _categoryRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(category);

            _categoryRepoMock.Setup(repo => repo.UpdateAsync(category))
                .Returns(Task.CompletedTask);

            var result = await _categoryService.ToggleCategoryVisibilityAsync(1);

            Assert.True(result);
            Assert.True(category.IsHidden);
        }

        [Fact]
        public async Task ToggleCategoryVisibilityAsync_WithInvalidId_ReturnsFalse()
        {
            _categoryRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Category)null!);

            var result = await _categoryService.ToggleCategoryVisibilityAsync(99);

            Assert.False(result);
        }
    }

}

