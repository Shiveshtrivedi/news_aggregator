using news_aggregator.console.Http;
using news_aggregator.console.Menu.Handler;
using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu
{
    public class NotificationMenu : IMenu
    {
        private readonly INotificationService _notificationService;
        private readonly string _userName;
        private readonly NotificationHandler _notificationHandler;

        public NotificationMenu(INotificationService notificationService, string userName)
        {
            _notificationService = notificationService;
            _userName = userName;
            _notificationHandler = new NotificationHandler(_notificationService, _userName);
        }

        public async Task Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome to News Application, {_userName}! Date: {DateTime.Today:dd-MMM-yyyy} \nTime: {DateTime.Now:hh:mmtt}");
                Console.WriteLine("N O T I F I C A T I O N S");
                Console.WriteLine("1. View Notifications");
                Console.WriteLine("2. Configure Notifications");
                Console.WriteLine("3. Back");
                Console.WriteLine("4. Logout");
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await _notificationHandler.ShowNotificationsAsync();
                        break;
                    case "2":
                        await _notificationHandler.ShowConfigurationMenuAsync();
                        break;
                    case "3":
                        return;
                    case "4":
                        Session.Logout();
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
