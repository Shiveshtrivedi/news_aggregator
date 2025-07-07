using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException.NewsArticle;
using news_application.Context;
using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(NewsDataContext context) : base(context) { }

        public async Task DeleteOlderThanAsync(DateTime cutoffDate)
        {
            try
            {
                var oldArticles = _context.NewsArticles
                    .Where(newsArticle => newsArticle.PublishedAt < cutoffDate);

                _context.NewsArticles.RemoveRange(oldArticles);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new NewsArticleOperationException("Failed to delete old articles.", ex);
            }
        }

        public async Task<bool> ExistsAsync(string title, string url)
        {
            try
            {
                return await _context.NewsArticles.AnyAsync(article => article.Title == title && article.Url == url);
            }
            catch (Exception ex)
            {
                throw new NewsArticleOperationException("Failed to check article existence.", ex);
            }
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsByTitleAsync(string title, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var query = _context.NewsArticles.AsQueryable();

                if (!string.IsNullOrEmpty(title))
                    query = query.Where(newsArticle => newsArticle.Title.Contains(title));

                if (startDate.HasValue)
                    query = query.Where(newsArticle => newsArticle.PublishedAt >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(newsArticle => newsArticle.PublishedAt <= endDate.Value);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new NewsArticleOperationException("Failed to search articles by title.", ex);
            }
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(string category)
        {
            try
            {
                if (!Enum.TryParse<CategoryType>(category, true, out var parsedCategory))
                    return new List<NewsArticle>();

                return await _context.NewsArticles
                    .Where(newsArticle => newsArticle.Category == parsedCategory)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new NewsArticleOperationException("Failed to get articles by category.", ex);
            }
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.NewsArticles
                    .Where(article => article.PublishedAt >= startDate && article.PublishedAt <= endDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new NewsArticleOperationException("Failed to get articles by date range.", ex);
            }
        }

        public async Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (!Enum.TryParse<CategoryType>(category, true, out var categoryEnum))
                    throw new ArgumentException($"Invalid category: {category}");

                var query = _context.NewsArticles.Where(news => news.Category == categoryEnum);

                var start = startDate?.Date ?? DateTime.MinValue;
                var end = endDate?.Date.AddDays(1) ?? DateTime.MaxValue;

                query = query.Where(news => news.PublishedAt >= start && news.PublishedAt < end);

                return await query
                    .Select(news => new NewsArticleDto
                    {
                        NewsArticleId = news.NewsArticleId,
                        Title = news.Title,
                        Content = news.Content,
                        Category = news.Category.ToString(),
                        PublishedAt = news.PublishedAt,
                        Url = news.Url,
                        Source = news.Source,
                        Likes = news.Likes,
                        DisLikes = news.Dislikes
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new NewsArticleOperationException("Failed to get articles by category and date range.", ex);
            }
        }
    }
}
