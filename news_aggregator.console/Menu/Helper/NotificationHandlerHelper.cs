using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;

namespace news_aggregator.console.Menu.Handler
{
    public static class NotificationHandlerHelper
    {
        public static async Task DisplayUserNotificationsAsync(INotificationService service)
        {
            try
            {
                var config = await service.GetConfigAsync(Session.UserId);
                Console.Clear();
                Console.WriteLine("Your Notifications:\n");
                foreach (var setting in config.CategorySettings)
                {
                    Console.WriteLine($"- {setting.CategoryName}: {(setting.IsEnabled ? "Enabled" : "Disabled")}");
                }
                Console.WriteLine($"- Keywords: {(config.KeywordsEnabled ? "Enabled" : "Disabled")}");
            }
            catch (NotificationServiceException ex)
            {
                Console.WriteLine($"Error while loading notifications: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        public static async Task<NotificationConfigDto?> FetchConfigAsync(INotificationService service)
        {
            try
            {
                return await service.GetConfigAsync(Session.UserId);
            }
            catch (NotificationServiceException ex)
            {
                Console.WriteLine($"Failed to load configuration: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            Console.ReadKey();
            return null;
        }

        public static void DisplayConfiguration(NotificationConfigDto config, string userName)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to News Application, {userName}! Date: {DateTime.Today:dd-MMM-yyyy} \nTime:{DateTime.Now:hh:mmtt}");
            Console.WriteLine("C O N F I G U R E - N O T I F I C A T I O N S");

            for (int i = 0; i < config.CategorySettings.Count; i++)
            {
                var setting = config.CategorySettings[i];
                Console.WriteLine($"{i + 1}. {setting.CategoryName} - {(setting.IsEnabled ? "Enabled" : "Disabled")}");
            }

            Console.WriteLine($"{config.CategorySettings.Count + 1}. Back");
            Console.WriteLine($"{config.CategorySettings.Count + 2}. Logout");
            Console.Write("Enter your option: ");
        }

        public static bool HandleMenuNavigation(int option, int categoryCount)
        {
            if (option == categoryCount + 1)
            {
                return true;
            }

            if (option == categoryCount + 2)
            {
                Environment.Exit(0);
            }

            return false;
        }

        public static async Task HandleCategoryToggleAsync(INotificationService service, CategorySettingDto category)
        {
            try
            {
                if (category.CategoryName.Equals("Keywords", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter keywords separated by commas: ");
                    var keywordInput = Console.ReadLine();
                    await service.SubmitKeywordsAsync(Session.UserId, keywordInput ?? "");
                    Console.WriteLine("Keywords updated. Press any key to continue...");
                }
                else
                {
                    var newStatus = !category.IsEnabled;
                    await service.ToggleCategoryAsync(Session.UserId, category.CategoryName, newStatus);
                    Console.WriteLine($"{category.CategoryName} notifications {(newStatus ? "enabled" : "disabled")}. Press any key to continue...");
                }
            }
            catch (NotificationServiceException ex)
            {
                Console.WriteLine($"Failed to update: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
