using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface INotificationConfigRepository
    {
        Task<NotificationConfig?> GetByUserAsync(int userId);
        Task AddOrUpdateAsync(NotificationConfig config);
        Task<NotificationConfig> GetOrCreateAsync(int userId);
    }
}
