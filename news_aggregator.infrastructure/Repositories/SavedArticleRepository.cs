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
    public class SavedArticleRepository : ISavedArticleRepository
    {
        private readonly NewsDataContext _context;

        public SavedArticleRepository(NewsDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllSavedArticlesAsync()
        {
            var savedArticles = await _context.SavedArticles
                                            .Include(savedArticles => savedArticles.NewsArticle)
                                            .ToListAsync();

            return savedArticles.Select(savedArticles => savedArticles.NewsArticle);
        }


        public async Task<IEnumerable<NewsArticle>> GetSavedArticlesByUserIdAsync(int userId)
        {
            var savedArticles = await _context.SavedArticles
                .Where(savedArticle => savedArticle.UserId == userId)
                .Include(savedArticle => savedArticle.NewsArticle)
                .ToListAsync();

            return savedArticles.Select(savedArticle => savedArticle.NewsArticle);
        }

        public async Task SaveArticleAsync(int userId, int newsArticleId)
        {
            var savedArticle = new SavedArticle
            {
                UserId = userId,
                NewsArticleId = newsArticleId,
                SavedOn = DateTime.UtcNow
            };

            _context.SavedArticles.Add(savedArticle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSavedArticleAsync(int userId, int newsArticleId)
        {
            var savedArticle = await _context.SavedArticles
                .FirstOrDefaultAsync(savedArticle => savedArticle.UserId == userId && savedArticle.NewsArticleId == newsArticleId);

            if (savedArticle != null)
            {
                _context.SavedArticles.Remove(savedArticle);
                await _context.SaveChangesAsync();
            }
        }

    }
}
