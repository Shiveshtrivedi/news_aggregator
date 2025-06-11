using Microsoft.Extensions.Configuration;
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
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public NotificationService(INotificationRepository notificationRepository, IConfiguration configuration, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task CreateNotificationAsync(int userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                SentAt = DateTime.UtcNow
            };
            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _notificationRepository.GetUserNotificationsAsync(userId);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new System.Net.Mail.SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]))
            {
                Credentials = new System.Net.NetworkCredential(_configuration["Smtp:User"], _configuration["Smtp:Password"]),
                EnableSsl = true
            };

            var mailMessage = new System.Net.Mail.MailMessage(_configuration["Smtp:Email"], toEmail, subject, body)
            {
                IsBodyHtml = false
            };

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task NotifyUserAsync(int userId, string message)
        {
            var user = await _userRepository.GetByIdAsync(userId);    
            if (user == null)
                throw new Exception("User not found");

            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            await _notificationRepository.AddNotificationAsync(notification);
            await SendEmailAsync(user.Email, "New Notification", message);
        }

    }
}
