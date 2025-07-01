using news_aggregator.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface IUserArticleInteractionRepository
    {
        Task<UserArticleInteraction?> GetInteractionAsync(int userId, int articleId);
        Task AddOrUpdateInteractionAsync(UserArticleInteraction interaction);
        Task IncrementLikesAsync(int articleId);
        Task DecrementLikesAsync(int articleId);
        Task IncrementDislikesAsync(int articleId);
        Task DecrementDislikesAsync(int articleId);
    }

}
