using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models;
using news_application.Context;
using news_aggregator.shared.CustomException.NewsArticle;
using news_aggregator.shared.CustomException;

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
            try
            {
                return await _context.UserArticleInteractions
                    .Include(x => x.NewsArticle)
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.NewsArticleId == articleId);
            }
            catch (Exception ex)
            {
                throw new UserArticleInteractionException("Failed to retrieve interaction.", ex);
            }
        }

        public async Task AddOrUpdateInteractionAsync(UserArticleInteraction interaction)
        {
            try
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
            catch (Exception ex)
            {
                throw new UserArticleInteractionException("Failed to add or update interaction.", ex);
            }
        }

        public async Task IncrementLikesAsync(int articleId)
        {
            try
            {
                var article = await _context.NewsArticles.FindAsync(articleId);
                if (article != null)
                {
                    article.Likes++;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new UserArticleInteractionException("Failed to increment likes.", ex);
            }
        }

        public async Task DecrementLikesAsync(int articleId)
        {
            try
            {
                var article = await _context.NewsArticles.FindAsync(articleId);
                if (article != null && article.Likes > 0)
                {
                    article.Likes--;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new UserArticleInteractionException("Failed to decrement likes.", ex);
            }
        }

        public async Task IncrementDislikesAsync(int articleId)
        {
            try
            {
                var article = await _context.NewsArticles.FindAsync(articleId);
                if (article != null)
                {
                    article.Dislikes++;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new UserArticleInteractionException("Failed to increment dislikes.", ex);
            }
        }

        public async Task DecrementDislikesAsync(int articleId)
        {
            try
            {
                var article = await _context.NewsArticles.FindAsync(articleId);
                if (article != null && article.Dislikes > 0)
                {
                    article.Dislikes--;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new UserArticleInteractionException("Failed to decrement dislikes.", ex);
            }
        }

        public async Task<List<int>> GetLikedArticleIdsAsync(int userId)
        {
            try
            {
                return await _context.UserArticleInteractions
                    .Where(i => i.UserId == userId && i.IsLiked)
                    .Select(i => i.NewsArticleId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new UserArticleInteractionException("Failed to retrieve liked article IDs.", ex);
            }
        }
    }
}
