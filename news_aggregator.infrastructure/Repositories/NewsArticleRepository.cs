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
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        private readonly NewsDataContext _context;

        public NewsArticleRepository(NewsDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeleteOlderThanAsync(DateTime cutoffDate)
        {
            var oldArticles = _context.NewsArticles
                .Where(a => a.PublishedAt < cutoffDate);

            _context.NewsArticles.RemoveRange(oldArticles);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string title, string url)
        {
            return await _context.NewsArticles.AnyAsync(a => a.Title == title && a.Url == url);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsByTitleAsync(string title, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.NewsArticles.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(n => n.Title.Contains(title));

            if (startDate.HasValue)
                query = query.Where(n => n.PublishedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(n => n.PublishedAt <= endDate.Value);

            return await query.ToListAsync();
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

        public async Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime startDate, DateTime endDate)
        {
            if (!Enum.TryParse<CategoryType>(category, true, out var categoryEnum))
                throw new ArgumentException("Invalid category.");
            var articles = await _context.NewsArticles.ToListAsync();  
            return articles.Select(n => new NewsArticleDto
            {
                NewsArticleId = n.NewsArticleId,
                Title = n.Title,
                Content = n.Content,
                Category = n.Category.ToString(),     
                PublishedAt = n.PublishedAt
            }).ToList();
        }

    }
}
