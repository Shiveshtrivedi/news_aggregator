using news_aggregator.console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler.Interface
{
    public interface IHeadlinesCategoryHandler
    {
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<List<NewsArticleDto>> GetArticlesAsync(string category, DateTime start, DateTime end);
        Task SaveArticleAsync(int userId, int articleId);

    }
}
