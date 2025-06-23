using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.shared.Validation;
using news_application.Context;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly NewsDataContext _context;

        public CategoryRepository(NewsDataContext context) : base(context) 
        {
            _context = context;
        }
       
        public async Task<bool> ExistsAsync(string categoryName)
        {
            return await _context.Categories
                .AnyAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());
        }

    }
}
