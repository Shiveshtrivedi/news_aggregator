using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models;
using news_aggregator.shared.CustomException;
using news_aggregator.shared.CustomException.NewsArticle;

namespace news_aggregator.application.News
{
    public class NewsInteractionService : INewsInteractionService
    {
        private readonly INewsArticleRepository _articleRepository;
        private readonly IUserArticleInteractionRepository _interactionRepository;

        public NewsInteractionService(
            INewsArticleRepository articleRepository,
            IUserArticleInteractionRepository interactionRepository)
        {
            _articleRepository = articleRepository;
            _interactionRepository = interactionRepository;
        }

        public async Task ToggleLikeAsync(int articleId, int userId)
        {
            try
            {
                var article = await _articleRepository.GetByIdAsync(articleId);
                if (article == null) throw new NewsArticleNotFoundException("Article not found");

                var interaction = await _interactionRepository.GetInteractionAsync(userId, articleId);

                if (interaction == null)
                {
                    interaction = new UserArticleInteraction
                    {
                        UserId = userId,
                        NewsArticleId = articleId,
                        IsLiked = true,
                        IsDisliked = false
                    };
                    await _interactionRepository.IncrementLikesAsync(articleId);
                }
                else if (interaction.IsLiked)
                {
                    interaction.IsLiked = false;
                    await _interactionRepository.DecrementLikesAsync(articleId);
                }
                else
                {
                    interaction.IsLiked = true;
                    if (interaction.IsDisliked)
                    {
                        interaction.IsDisliked = false;
                        await _interactionRepository.DecrementDislikesAsync(articleId);
                    }
                    await _interactionRepository.IncrementLikesAsync(articleId);
                }

                await _interactionRepository.AddOrUpdateInteractionAsync(interaction);
            }
            catch (UserArticleInteractionException)
            {
                throw;
            }
        }

        public async Task ToggleDislikeAsync(int articleId, int userId)
        {
            try
            {
                var article = await _articleRepository.GetByIdAsync(articleId);
                if (article == null) throw new NewsArticleNotFoundException("Article not found");

                var interaction = await _interactionRepository.GetInteractionAsync(userId, articleId);

                if (interaction == null)
                {
                    interaction = new UserArticleInteraction
                    {
                        UserId = userId,
                        NewsArticleId = articleId,
                        IsLiked = false,
                        IsDisliked = true
                    };
                    await _interactionRepository.IncrementDislikesAsync(articleId);
                }
                else if (interaction.IsDisliked)
                {
                    interaction.IsDisliked = false;
                    await _interactionRepository.DecrementDislikesAsync(articleId);
                }
                else
                {
                    interaction.IsDisliked = true;
                    if (interaction.IsLiked)
                    {
                        interaction.IsLiked = false;
                        await _interactionRepository.DecrementLikesAsync(articleId);
                    }
                    await _interactionRepository.IncrementDislikesAsync(articleId);
                }

                await _interactionRepository.AddOrUpdateInteractionAsync(interaction);
            }
            catch (UserArticleInteractionException)
            {
                throw;
            }
        }
    }
}
