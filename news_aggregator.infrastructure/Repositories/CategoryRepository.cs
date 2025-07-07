using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.shared.CustomException.CategoryException;
using news_application.Context;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(NewsDataContext context) : base(context) { }

        public async Task<bool> CategoryExistsAsync(string categoryName)
        {
            try
            {
                return await _context.Categories
                    .AnyAsync(category => category.CategoryName.ToLower() == categoryName.ToLower());
            }
            catch (Exception ex)
            {
                throw new CategoryOperationException("Error while checking if category exists.", ex);
            }
        }

        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());

                if (category == null)
                    throw new CategoryNotFoundException(categoryName);

                return category;
            }
            catch (CategoryNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CategoryOperationException($"Error while retrieving category '{categoryName}'.", ex);
            }
        }
    }
}
