using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task CreateNotificationAsync(int userId, string message);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task NotifyUserAsync(int userId, string message);
        Task NotifyAdminAsync(string messageHtml);
    }
}
