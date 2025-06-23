using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.shared.CustomException;
using news_aggregator.shared.CustomException.CategoryException;

namespace news_aggregator.application
{
    public class NotificationConfigService : INotificationConfigService
    {
        private readonly INotificationConfigRepository _notificationConfigRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotificationService _notificationService;
        private readonly INewsQueryService _newsQueryService;
        private readonly INotificationHtmlBuilder _notificationHtmlBuilder;

        public NotificationConfigService(INotificationConfigRepository notificationConfigRepository, ICategoryRepository categoryRepository, INotificationService notificationService,INewsQueryService newsQueryService, INotificationHtmlBuilder notificationHtmlBuilder)
        {
            _notificationConfigRepository = notificationConfigRepository;
            _categoryRepository = categoryRepository;
            _notificationService = notificationService;
            _newsQueryService = newsQueryService;
            _notificationHtmlBuilder = notificationHtmlBuilder;
        }

        public async Task<NotificationConfig> GetOrCreateForUserAsync(int userId)
        {
            return await _notificationConfigRepository.GetOrCreateAsync(userId);
        }

        public async Task UpdateConfigAsync(NotificationConfig config)
        {
            await _notificationConfigRepository.AddOrUpdateAsync(config);
        }

        public async Task ToggleCategoryAsync(int userId, string category, bool enable)
        {
            var config = await _notificationConfigRepository.GetOrCreateAsync(userId);

            if (IsKeywordCategory(category))
            {
                ToggleKeywordNotification(config, enable);
            }
            else
            {
                await ToggleCategoryNotificationAsync(config, userId, category, enable);
            }

            await _notificationConfigRepository.AddOrUpdateAsync(config);

            if (enable && !IsKeywordCategory(category))
            {
                await SendCategoryNewsNotificationAsync(userId, category);
            }
        }

        private async Task ToggleCategoryNotificationAsync(NotificationConfig config, int userId, string category, bool enable)
        {
            var isValidCategory = await _categoryRepository.ExistsAsync(category);
            if (!isValidCategory)
                throw new CategoryNotFoundException("category not found");

            var setting = config.CategorySettings
                .FirstOrDefault(s => s.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (setting != null)
            {
                setting.IsEnabled = enable;
            }
            else
            {
                config.CategorySettings.Add(new NotificationCategorySetting
                {
                    CategoryName = category,
                    IsEnabled = enable,
                    UserId = userId
                });
            }
        }


        private bool IsKeywordCategory(string category)
        {
            return category.Equals("keywords", StringComparison.OrdinalIgnoreCase);
        }

        private void ToggleKeywordNotification(NotificationConfig config, bool enable)
        {
            config.KeywordsEnabled = enable;
        }

        private async Task SendCategoryNewsNotificationAsync(int userId, string category)
        {
            var newsArticles = await _newsQueryService.GetNewsByCategoryAsync(category);
            var message = _notificationHtmlBuilder.Build(category, newsArticles.Take(5));
            await _notificationService.NotifyUserAsync(userId, message);
        }

    }

}
