using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Services;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("notify")]
        public async Task<IActionResult> NotifyUser(int userId, string message)
        {
            await _notificationService.NotifyUserAsync(userId, message);
            return Ok("Notification created and email sent.");
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

    }
}
