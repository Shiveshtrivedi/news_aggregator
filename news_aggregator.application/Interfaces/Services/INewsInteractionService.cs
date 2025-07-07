using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface INewsInteractionService
    {
        Task ToggleLikeAsync(int articleId, int userId);
        Task ToggleDislikeAsync(int articleId, int userId);
    }
}
