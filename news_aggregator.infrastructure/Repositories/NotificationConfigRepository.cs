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
            var existingNotification = await GetByUserAsync(config.UserId);

            if (existingNotification == null)
            {
                await _context.NotificationConfigs.AddAsync(config);
            }
            else
            {
                _context.Entry(existingNotification).CurrentValues.SetValues(config);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<NotificationConfig?> GetByUserAsync(int userId)
        {
            return await _context.NotificationConfigs
               .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<NotificationConfig> GetOrCreateAsync(int userId)
        {
            var config = await GetByUserAsync(userId);
            if (config == null)
            {
                config = new NotificationConfig
                {
                    UserId = userId,
                    BusinessEnabled = true,
                    EntertainmentEnabled = true,
                    SportsEnabled = false,
                    TechnologyEnabled = false,
                    KeywordsEnabled = true
                };

                await _context.NotificationConfigs.AddAsync(config);
                await _context.SaveChangesAsync();
            }
            return config;
        }
    }
}
