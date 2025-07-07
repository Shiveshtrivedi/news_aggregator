using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<news_application.Models.Category>
    {
        Task<bool> CategoryExistsAsync(string categoryName);
        Task<news_application.Models.Category> GetCategoryByNameAsync(string categoryName);

    }

}
