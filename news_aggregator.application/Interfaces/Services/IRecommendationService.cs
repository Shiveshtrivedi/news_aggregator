using news_aggregator.domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface IRecommendationService
    {
        Task<List<NewsArticleDto>> GetPersonalizedArticlesAsync(int userId);
    }
}
