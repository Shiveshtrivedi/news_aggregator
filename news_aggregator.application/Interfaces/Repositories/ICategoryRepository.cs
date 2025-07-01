using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> CategoryExistsAsync(string categoryName);
        Task<Category?> GetCategoryByNameAsync(string categoryName);

    }

}
