using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class NewsQueryService : INewsQueryService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly IUserArticleInteractionRepository _userArticleInteractionRepository;

        public NewsQueryService(INewsArticleRepository newsArticleRepository, IUserArticleInteractionRepository userArticleInteractionRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _userArticleInteractionRepository = userArticleInteractionRepository;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsAsync()
        {
            return await _newsArticleRepository.GetAllAsync();
        }

        public async Task<NewsArticle?> GetNewsByIdAsync(int articleId)
        {
            return await _newsArticleRepository.GetByIdAsync(articleId);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsByTitleAsync(string title, DateTime? startDate, DateTime? endDate)
        {
            return await _newsArticleRepository.SearchNewsByTitleAsync(title, startDate, endDate);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(string category)
        {

            return await _newsArticleRepository.GetNewsByCategoryAsync(category);
        }
        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _newsArticleRepository.GetNewsByDateRangeAsync(startDate, endDate);
        }

        public async Task<List<NewsArticleWithUserInteractionDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime? startDate, DateTime? endDate,int userId)
        {
            var articleDtos = await _newsArticleRepository.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);

            var result = new List<NewsArticleWithUserInteractionDto>();

            foreach (var article in articleDtos)
            {
                var interaction = await _userArticleInteractionRepository.GetInteractionAsync(userId, article.NewsArticleId);

                result.Add(new NewsArticleWithUserInteractionDto
                {
                    NewsArticleId = article.NewsArticleId,
                    Title = article.Title,
                    Content = article.Content,
                    Category = article.Category,
                    PublishedAt = article.PublishedAt,
                    IsLikedByUser = interaction?.IsLiked ?? false,
                    IsDislikedByUser = interaction?.IsDisliked ?? false,
                    Likes = interaction?.NewsArticle?.Likes ?? 0,           
                    Dislikes = interaction?.NewsArticle?.Dislikes ?? 0      
                });
            }

            return result;
        }
    }
}
