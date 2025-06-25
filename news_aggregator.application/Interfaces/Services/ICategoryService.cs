using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.domain.Models.DTOs;

namespace news_aggregator.application.Interfaces.Services
{

    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<bool> ToggleCategoryVisibilityAsync(int categoryId);

    }

}
