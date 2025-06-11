using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_application.Context;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NewsDataContext _context;

        public CategoryRepository(NewsDataContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            var existingCategory = await _context.Categories.FindAsync(categoryId);

            if (existingCategory == null)
                return false;

            _context.Categories.Remove(existingCategory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
