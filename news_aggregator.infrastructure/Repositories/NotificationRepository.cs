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
    public class NotificationRepository : INotificationRepository
    {
        private readonly NewsDataContext _context;

        public NotificationRepository(NewsDataContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new NotificationRepositoryOperationException("Failed to add notification.", ex);
            }
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.SentAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new NotificationRepositoryOperationException("Failed to fetch user notifications.", ex);
            }
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new NotificationRepositoryOperationException("Failed to mark notification as read.", ex);
            }
        }
    }

}
