using AutoMapper;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;
using news_aggregator.shared.CustomException.NewsArticle;
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
        private readonly IReportArticleRepository _reportArticleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public NewsQueryService(INewsArticleRepository newsArticleRepository, IUserArticleInteractionRepository userArticleInteractionRepository, IMapper mapper, IReportArticleRepository reportArticleRepository, ICategoryRepository categoryRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _userArticleInteractionRepository = userArticleInteractionRepository;
            _mapper = mapper;
            _reportArticleRepository = reportArticleRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<NewsArticleDto>> GetAllNewsAsync()
        {
            var articles = await _newsArticleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
        }

        public async Task<NewsArticleDto?> GetNewsByIdAsync(int articleId)
        {
            var article = await _newsArticleRepository.GetByIdAsync(articleId);
            if (article == null)
                throw new NewsArticleNotFoundException($"Article with ID {articleId} not found.");

            return _mapper.Map<NewsArticleDto>(article);
        }

        public async Task<IEnumerable<NewsArticleDto>> SearchNewsByTitleAsync(string title, DateTime? startDate, DateTime? endDate)
        {
            var articles = await _newsArticleRepository.SearchNewsByTitleAsync(title, startDate, endDate);
            return _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
        }

        public async Task<IEnumerable<NewsArticleDto>> GetNewsByCategoryAsync(string category)
        {

            var articles = await _newsArticleRepository.GetNewsByCategoryAsync(category);
            return _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
        }
        public async Task<IEnumerable<NewsArticleDto>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var articles = await _newsArticleRepository.GetNewsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
        }

        public async Task<List<NewsArticleWithUserInteractionDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime? startDate, DateTime? endDate,int userId)
        {
            var articleDtos = await _newsArticleRepository.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);

            var result = new List<NewsArticleWithUserInteractionDto>();

            foreach (var article in articleDtos)
            {
                var articleCategory = await _categoryRepository.GetByNameAsync(article.Category);

                if(articleCategory?.IsHidden == true)
                    continue; 

                var reportCount = await _reportArticleRepository.GetReportCountAsync(article.NewsArticleId);
                if (reportCount > 3 || article.IsHidden)
                    continue;

                var interaction = await _userArticleInteractionRepository.GetInteractionAsync(userId, article.NewsArticleId);

                result.Add(new NewsArticleWithUserInteractionDto
                {
                    NewsArticleId = article.NewsArticleId,
                    Title = article.Title,
                    Content = article.Content,
                    Url = article.Url,
                    Source = article.Source,
                    Category = article.Category,
                    PublishedAt = article.PublishedAt,
                    IsLikedByUser = interaction?.IsLiked ?? false,
                    IsDislikedByUser = interaction?.IsDisliked ?? false,
                    Likes = article.Likes,           
                    Dislikes = article.DisLikes
                });
            }

            return result;
        }
    }
}
