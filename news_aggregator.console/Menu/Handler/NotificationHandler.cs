using news_aggregator.console.Http;
using news_aggregator.console.Services.Interfaces;
using news_aggregator.console.Exceptions;
using System;
using System.Threading.Tasks;
using news_aggregator.console.Menu.Handler.Interface;
using news_aggregator.console.Models;

namespace news_aggregator.console.Menu.Handler
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly INotificationService _notificationService;
        private readonly string _userName;

        public NotificationHandler(INotificationService notificationService, string userName)
        {
            _notificationService = notificationService;
            _userName = userName;
        }

        public async Task ShowNotificationsAsync()
        {
            await NotificationHandlerHelper.DisplayUserNotificationsAsync(_notificationService);
            Console.WriteLine("\n1. Back");
            Console.Write("Choose an option: ");
            var input = Console.ReadLine();
            if (input != "1")
            {
                Console.WriteLine("Invalid option. Press any key to try again...");
                Console.ReadKey();
            }
        }

        public async Task ShowConfigurationMenuAsync()
        {
            while (true)
            {
                var config = await NotificationHandlerHelper.FetchConfigAsync(_notificationService);
                if (config == null) continue;

                NotificationHandlerHelper.DisplayConfiguration(config, _userName);
                var input = Console.ReadLine();

                if (!int.TryParse(input, out int option))
                {
                    Console.WriteLine("Invalid input. Press any key to try again...");
                    Console.ReadKey();
                    continue;
                }

                if (NotificationHandlerHelper.HandleMenuNavigation(option, config.CategorySettings.Count)) return;

                if (option >= 1 && option <= config.CategorySettings.Count)
                {
                    var selectedCategory = config.CategorySettings[option - 1];
                    await NotificationHandlerHelper.HandleCategoryToggleAsync(_notificationService, selectedCategory);
                }
                else
                {
                    Console.WriteLine("Invalid option. Press any key to try again...");
                    Console.ReadKey();
                }
            }
        }
    }
}
