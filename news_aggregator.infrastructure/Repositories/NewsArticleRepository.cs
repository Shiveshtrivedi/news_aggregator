using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models.DTOs;
using news_application.Context;
using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly NewsDataContext _context;

        public NewsArticleRepository(NewsDataContext context)
        {
            _context = context;
        }
        public async Task AddAsync(NewsArticle article)
        {
            _context.NewsArticles.Add(article);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int newsArticleId)
        {
            var article = await _context.NewsArticles.FindAsync(newsArticleId);
            if (article != null)
            {
                _context.NewsArticles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<news_application.Models.NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles.OrderByDescending(date => date.PublishedAt).ToListAsync(); 
        }

        public async Task<news_application.Models.NewsArticle?> GetByIdAsync(int articleId)
        {
            return await _context.NewsArticles.FindAsync(articleId);
        }

        public async Task DeleteOlderThanAsync(DateTime cutoffDate)
        {
            var oldArticles = _context.NewsArticles
                .Where(a => a.PublishedAt < cutoffDate);

            _context.NewsArticles.RemoveRange(oldArticles);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string title)
        {
            return await _context.NewsArticles.AnyAsync(a => a.Title == title);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsByTitleAsync(string title)
        {
            return await _context.NewsArticles.Where(n => n.Title.Contains(title)).ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(string category)
        {
            if (!Enum.TryParse<CategoryType>(category, true, out var parsedCategory))
                return new List<NewsArticle>();

            return await _context.NewsArticles.Where(n => n.Category == parsedCategory).ToListAsync();
        }
        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Where(article => article.PublishedAt >= startDate && article.PublishedAt <= endDate)
                .ToListAsync();
        }

    }
}
