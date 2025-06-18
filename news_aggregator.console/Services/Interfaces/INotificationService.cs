using news_aggregator.console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<string>> GetNotificationsAsync(int userId);
        Task<NotificationConfigDto> GetConfigAsync(int userId);
        Task ToggleCategoryAsync(int userId, string category, bool enable);
        Task SubmitKeywordsAsync(int userId, string keywords);
    }
}
