using news_aggregator.console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<bool> AddCategoryAsync(string categoryName);
        Task<bool> ToggleCategoryVisibilityAsync(int categoryId);

    }
}
