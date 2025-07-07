using news_aggregator.domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public class CategoryMockDataGenerator
    {
        public static CategoryDto GetTestCategoryDto(int id = 1, string name = "technology")
        {
            return new CategoryDto
            {
                CategoryId = id,
                Name = name
            };
        }

        public static List<CategoryDto> GetTestCategoryDtoList()
        {
            return new List<CategoryDto>
            {
                new CategoryDto { CategoryId = 1, Name = "business" },
                new CategoryDto { CategoryId = 2, Name = "sports" },
                new CategoryDto { CategoryId = 3, Name = "technology" },
                new CategoryDto { CategoryId = 4, Name = "entertainment" }
            };
        }

        public static CreateCategoryDto GetCreateCategoryDto(string name = "general")
        {
            return new CreateCategoryDto
            {
                CategoryName = name
            };
        }
    }
}
