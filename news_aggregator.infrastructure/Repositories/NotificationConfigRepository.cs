using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
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
            var existingConfig = await _context.NotificationConfigs
               .Include(n => n.CategorySettings)
               .FirstOrDefaultAsync(n => n.UserId == config.UserId);

            if (existingConfig == null)
            {
                await _context.NotificationConfigs.AddAsync(config);
            }
            else
            {
                existingConfig.KeywordsEnabled = config.KeywordsEnabled;

                foreach (var updatedSetting in config.CategorySettings)
                {
                    var existingSetting = existingConfig.CategorySettings
                        .FirstOrDefault(s => s.CategoryName.ToLower() == updatedSetting.CategoryName.ToLower());

                    if (existingSetting != null)
                    {
                        existingSetting.IsEnabled = updatedSetting.IsEnabled;
                    }
                    else
                    {
                        existingConfig.CategorySettings.Add(new NotificationCategorySetting
                        {
                            CategoryName = updatedSetting.CategoryName,
                            IsEnabled = updatedSetting.IsEnabled,
                            UserId = config.UserId
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<NotificationConfig?> GetByUserAsync(int userId)
        {
            return await _context.NotificationConfigs
                .Include(n => n.CategorySettings)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<NotificationConfig> GetOrCreateAsync(int userId)
        {
            var config = await GetByUserAsync(userId);

            if (config == null)
            {
                var categories = await _context.Categories.ToListAsync();

                config = new NotificationConfig
                {
                    UserId = userId,
                    KeywordsEnabled = true,
                    CategorySettings = categories.Select(c => new NotificationCategorySetting
                    {
                        CategoryName = c.CategoryName,
                        IsEnabled = true,
                        UserId = userId
                    }).ToList()
                };

                await _context.NotificationConfigs.AddAsync(config);
                await _context.SaveChangesAsync();
            }
            else
            {
                var categories = await _context.Categories.ToListAsync();

                foreach (var category in categories)
                {
                    if (!config.CategorySettings.Any(cs => cs.CategoryName.ToLower() == category.CategoryName.ToLower()))
                    {
                        config.CategorySettings.Add(new NotificationCategorySetting
                        {
                            CategoryName = category.CategoryName,
                            IsEnabled = true,
                            UserId = userId
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }

            return config;
        }
        
    }
}
