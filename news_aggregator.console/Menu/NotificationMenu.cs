using news_aggregator.console.Http;
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

        public NotificationMenu(INotificationService notificationService, string userName)
        {
            _notificationService = notificationService;
            _userName = userName;
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
                        var config = await _notificationService.GetConfigAsync(Session.UserId);
                        Console.Clear();
                        Console.WriteLine("Your Notifications:\n");
                        foreach (var setting in config.CategorySettings)
                        {
                            Console.WriteLine($"- {setting.CategoryName}: {(setting.IsEnabled ? "Enabled" : "Disabled")}");
                        }
                        Console.WriteLine($"- Keywords: {(config.KeywordsEnabled ? "Enabled" : "Disabled")}");
                        Console.WriteLine("\n1. Back");
                        Console.Write("Choose an option: ");
                        var notifInput = Console.ReadLine();

                        if (notifInput == "1")
                            break;
                        else
                        {
                            Console.WriteLine("Invalid option. Press any key to try again...");
                            Console.ReadKey();
                        }
                        break;
                    case "2":
                        await ShowConfigurationMenu();

                        break;
                    case "3":
                        return;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }

        }

        private async Task ShowConfigurationMenu()
        {
            while (true)
            {
                var config = await _notificationService.GetConfigAsync(Session.UserId);
                Console.Clear();
                Console.WriteLine($"Welcome to News Application, {_userName}! Date: {DateTime.Today:dd-MMM-yyyy} \nTime:{DateTime.Now:hh:mmtt}");
                Console.WriteLine("C O N F I G U R E - N O T I F I C A T I O N S");

                var categories = config.CategorySettings;

                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i].CategoryName} - {(categories[i].IsEnabled ? "Enabled" : "Disabled")}");
                }

                Console.WriteLine($"{categories.Count + 1}. Back");
                Console.WriteLine($"{categories.Count + 2}. Logout");
                Console.Write("Enter your option: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int option))
                {
                    if (option >= 1 && option <= categories.Count)
                    {
                        var selectedCategory = categories[option - 1].CategoryName;

                        if (selectedCategory.Equals("Keywords", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.Write("Enter keywords separated by commas: ");
                            var keywordInput = Console.ReadLine();
                            await _notificationService.SubmitKeywordsAsync(Session.UserId, keywordInput ?? "");
                            Console.WriteLine("Keywords updated. Press any key to continue...");
                            Console.ReadKey();
                        }
                        else
                        {
                            bool currentStatus = categories[option - 1].IsEnabled;
                            bool newStatus = !currentStatus;
                            await _notificationService.ToggleCategoryAsync(Session.UserId, selectedCategory, newStatus);
                            Console.WriteLine($"{selectedCategory} notifications {(newStatus ? "enabled" : "disabled")}. Press any key to continue...");
                            Console.ReadKey();
                        }
                    }
                    else if (option == categories.Count + 1)
                    {
                        return;
                    }
                    else if (option == categories.Count + 2)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Press any key to try again...");
                    Console.ReadKey();
                }
            }
        }
    }
}
