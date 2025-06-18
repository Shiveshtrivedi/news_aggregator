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

        public NewsQueryService(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
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

        public async Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime startDate, DateTime endDate)
        {
            return await _newsArticleRepository.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);
        }
    }
}
