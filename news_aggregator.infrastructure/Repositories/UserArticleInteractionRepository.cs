using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models;
using news_application.Context;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class UserArticleInteractionRepository : IUserArticleInteractionRepository
    {
        private readonly NewsDataContext _context;

        public UserArticleInteractionRepository(NewsDataContext context)
        {
            _context = context;
        }

        public async Task<UserArticleInteraction?> GetInteractionAsync(int userId, int articleId)
        {
            return await _context.UserArticleInteractions
                .Include(newsArticle => newsArticle.NewsArticle)
                .FirstOrDefaultAsync(newsArticle => newsArticle.UserId == userId && newsArticle.NewsArticleId == articleId);
        }

        public async Task AddOrUpdateInteractionAsync(UserArticleInteraction interaction)
        {
            var existing = await GetInteractionAsync(interaction.UserId, interaction.NewsArticleId);

            if (existing == null)
            {
                _context.UserArticleInteractions.Add(interaction);
            }
            else
            {
                existing.IsLiked = interaction.IsLiked;
                existing.IsDisliked = interaction.IsDisliked;
                existing.Timestamp = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
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

        public async Task<List<int>> GetLikedArticleIdsAsync(int userId)
        {
            return await _context.UserArticleInteractions
                .Where(i => i.UserId == userId && i.IsLiked)
                .Select(i => i.NewsArticleId)
                .ToListAsync();
        }

    }

}
