using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.shared.CustomException;
using news_application.Context;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class NotificationConfigRepository : INotificationConfigRepository
    {
        private readonly NewsDataContext _context;

        public NotificationConfigRepository(NewsDataContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateAsync(NotificationConfig config)
        {
            try
            {
                var existingConfig = await _context.NotificationConfigs
                    .Include(x => x.CategorySettings)
                    .FirstOrDefaultAsync(x => x.UserId == config.UserId);

                if (existingConfig == null)
                {
                    await _context.NotificationConfigs.AddAsync(config);
                }
                else
                {
                    existingConfig.KeywordsEnabled = config.KeywordsEnabled;

                    foreach (var updated in config.CategorySettings)
                    {
                        var existing = existingConfig.CategorySettings
                            .FirstOrDefault(x => x.CategoryName.ToLower() == updated.CategoryName.ToLower());

                        if (existing != null)
                        {
                            existing.IsEnabled = updated.IsEnabled;
                        }
                        else
                        {
                            existingConfig.CategorySettings.Add(new NotificationCategorySetting
                            {
                                CategoryName = updated.CategoryName,
                                IsEnabled = updated.IsEnabled,
                                UserId = config.UserId
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new NotificationRepositoryOperationException("Failed to add or update notification config.", ex);
            }
        }

        public async Task<NotificationConfig?> GetByUserAsync(int userId)
        {
            try
            {
                return await _context.NotificationConfigs
                    .Include(x => x.CategorySettings)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new NotificationRepositoryOperationException("Failed to fetch user notification config.", ex);
            }
        }

        public async Task<NotificationConfig> GetOrCreateAsync(int userId)
        {
            try
            {
                var config = await GetByUserAsync(userId);

                if (config == null)
                {
                    config = await CreateDefaultNotificationConfigAsync(userId);
                    await _context.NotificationConfigs.AddAsync(config);
                }
                else
                {
                    await EnsureAllCategorySettingsExistAsync(config, userId);
                }

                await _context.SaveChangesAsync();
                return config;
            }
            catch (Exception ex)
            {
                throw new NotificationRepositoryOperationException("Failed to get or create user notification config.", ex);
            }
        }

        private async Task<NotificationConfig> CreateDefaultNotificationConfigAsync(int userId)
        {
            var categories = await _context.Categories.ToListAsync();

            return new NotificationConfig
            {
                UserId = userId,
                KeywordsEnabled = true,
                CategorySettings = categories.Select(x => new NotificationCategorySetting
                {
                    CategoryName = x.CategoryName,
                    IsEnabled = true,
                    UserId = userId
                }).ToList()
            };
        }

        private async Task EnsureAllCategorySettingsExistAsync(NotificationConfig config, int userId)
        {
            var categories = await _context.Categories.ToListAsync();

            foreach (var category in categories)
            {
                bool exists = config.CategorySettings
                    .Any(x => x.CategoryName.Equals(category.CategoryName, StringComparison.OrdinalIgnoreCase));

                if (!exists)
                {
                    config.CategorySettings.Add(new NotificationCategorySetting
                    {
                        CategoryName = category.CategoryName,
                        IsEnabled = true,
                        UserId = userId
                    });
                }
            }
        }
    }

}
