using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_application.Context;
using news_application.Models;
using news_aggregator.shared.CustomException;

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
            try
            {
                var savedArticles = await _context.SavedArticles
                    .Include(x => x.NewsArticle)
                    .ToListAsync();

                return savedArticles.Select(x => x.NewsArticle);
            }
            catch (Exception ex)
            {
                throw new GetSavedArticlesFailedException("Failed to retrieve all saved articles.", ex);
            }
        }

        public async Task<IEnumerable<NewsArticle>> GetSavedArticlesByUserIdAsync(int userId)
        {
            try
            {
                var savedArticles = await _context.SavedArticles
                    .Where(x => x.UserId == userId)
                    .Include(x => x.NewsArticle)
                    .ToListAsync();

                return savedArticles.Select(x => x.NewsArticle);
            }
            catch (Exception ex)
            {
                throw new GetSavedArticlesFailedException("Failed to retrieve saved articles by user.", ex);
            }
        }

        public async Task SaveArticleAsync(int userId, int newsArticleId)
        {
            try
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
            catch (Exception ex)
            {
                throw new SaveArticleFailedException("Failed to save article.", ex);
            }
        }

        public async Task DeleteSavedArticleAsync(int userId, int newsArticleId)
        {
            try
            {
                var savedArticle = await _context.SavedArticles
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.NewsArticleId == newsArticleId);

                if (savedArticle != null)
                {
                    _context.SavedArticles.Remove(savedArticle);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DeleteSavedArticleFailedException("Failed to delete saved article.", ex);
            }
        }
    }
}
