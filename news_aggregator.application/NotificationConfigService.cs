using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class NotificationConfigService : INotificationConfigService
    {
        private readonly INotificationConfigRepository _notificationConfigRepository;

        public NotificationConfigService(INotificationConfigRepository notificationConfigRepository)
        {
            _notificationConfigRepository = notificationConfigRepository;
        }

        public async Task<NotificationConfig> GetOrCreateForUserAsync(int userId)
        {
            return await _notificationConfigRepository.GetOrCreateAsync(userId);
        }

        public async Task UpdateConfigAsync(NotificationConfig config)
        {
            await _notificationConfigRepository.AddOrUpdateAsync(config);
        }
    }

}
