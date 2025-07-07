using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public class HeadlinesCategoryHandler
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsService;
        private readonly ISavedArticleService _savedArticleService;

        public HeadlinesCategoryHandler(ICategoryService categoryService,
            INewsArticleService newsService,
            ISavedArticleService savedArticleService)
        {
            _categoryService = categoryService;
            _newsService = newsService;
            _savedArticleService = savedArticleService;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            return await _categoryService.GetAllCategoriesAsync();
        }

        public async Task<List<NewsArticleDto>> GetArticlesAsync(string category, DateTime start, DateTime end)
        {
            return await _newsService.GetNewsByCategoryAndDateRangeAsync(category, start, end);
        }

        public async Task SaveArticleAsync(int userId, int articleId)
        {
            await _savedArticleService.SaveArticleAsync(userId, articleId);
        }
    }
}
