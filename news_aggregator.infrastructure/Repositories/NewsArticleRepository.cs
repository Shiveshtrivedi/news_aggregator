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
                .Where(newsArticle => newsArticle.PublishedAt < cutoffDate);

            _context.NewsArticles.RemoveRange(oldArticles);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string title, string url)
        {
            return await _context.NewsArticles.AnyAsync(article => article.Title == title && article.Url == url);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsByTitleAsync(string title, DateTime? startDate, DateTime? endDate)
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

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(string category)
        {
            if (!Enum.TryParse<CategoryType>(category, true, out var parsedCategory))
                return new List<NewsArticle>();

            return await _context.NewsArticles.Where(newsArticle => newsArticle.Category == parsedCategory).ToListAsync();
        }
        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Where(article => article.PublishedAt >= startDate && article.PublishedAt <= endDate)
                .ToListAsync();
        }

       public async Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime? startDate, DateTime? endDate)
        {
            if (!Enum.TryParse<CategoryType>(category, true, out var categoryEnum))
                throw new ArgumentException($"Invalid category: {category}");

            bool isStartNull = !startDate.HasValue || startDate.Value == DateTime.MinValue;
            bool isEndNull = !endDate.HasValue || endDate.Value == DateTime.MinValue;

            var query = _context.NewsArticles
                .Where(newsArticle => newsArticle.Category == categoryEnum);

            if (!isStartNull && !isEndNull)
            {
                var start = startDate.Value.Date;
                var end = endDate.Value.Date.AddDays(1);
                query = query.Where(newsArticle => newsArticle.PublishedAt >= start && newsArticle.PublishedAt < end);
            }
            else if (!isStartNull)
            {
                var start = startDate.Value.Date;
                var end = start.AddDays(1);
                query = query.Where(newsArticle => newsArticle.PublishedAt >= start && newsArticle.PublishedAt < end);
            }
            else if (!isEndNull)
            {
                var end = endDate.Value.Date.AddDays(1);
                query = query.Where(newsArticle => newsArticle.PublishedAt < end);
            }
            var articles = await query
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

            return articles;
        }

        public async Task<NewsArticle?> GetByIdAsync(int articleId)
        {
            return await _context.NewsArticles.FindAsync(articleId);
        }

        public async Task IncrementLikesAsync(int articleId)
        {
            var article = await _context.NewsArticles.FindAsync(articleId);
            if (article != null)
            {
                article.Likes++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DecrementLikesAsync(int articleId)
        {
            var article = await _context.NewsArticles.FindAsync(articleId);
            if (article != null && article.Likes > 0)
            {
                article.Likes--;
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementDislikesAsync(int articleId)
        {
            var article = await _context.NewsArticles.FindAsync(articleId);
            if (article != null)
            {
                article.Dislikes++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DecrementDislikesAsync(int articleId)
        {
            var article = await _context.NewsArticles.FindAsync(articleId);
            if (article != null && article.Dislikes > 0)
            {
                article.Dislikes--;
                await _context.SaveChangesAsync();
            }
        }
    }
}
