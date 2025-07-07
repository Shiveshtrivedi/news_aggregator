using Microsoft.Extensions.Configuration;
using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Menu.Handler;
using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Models;
using news_aggregator.console.Services;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu
{
    public class MainMenu : IMenu
    {
        private readonly IAuthService _authService;
        private readonly IServerService _serverService;
        private readonly ICategoryService _categoryService;
        private readonly INewsService _newsService;
        private readonly ISavedArticleService _savedArticleService;
        private readonly ISearchArticleService _searchArticleService;
        private readonly INotificationService _notificationService;
        private readonly IBlockedKeywordService _blockedKeywordService;
        private readonly AuthHandler _authHandler;
        private readonly AppServices _services;


        public MainMenu(AppServices services)
        {
            _services = services;

            _authHandler = new AuthHandler(
              _services.AuthService,
              _services.ServerService,
              _services.CategoryService,
              _services.NewsService,
              _services.SavedArticleService,
              _services.SearchArticleService,
              _services.NotificationService,
              _services.BlockedKeywordService,
              _services.UserKeywordService
              );

        }

        public async Task Show()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IHttpClientFactoryWrapper clientFactoryWrapper = new HttpClientFactory(configuration);

            bool exit = false;

            while (!exit)
            {
                try
                {
                    Session.Reset();

                    Console.Clear();
                    Console.WriteLine("Welcome to the News Aggregator application. Please choose the options below.");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Sign up");
                    Console.WriteLine("3. Exit");
                    Console.Write("Enter your choice: ");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            await _authHandler.HandleLoginAsync();
                            break;

                        case "2":
                            await _authHandler.HandleSignUpAsync();
                            break;

                        case "3":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid option. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }

                    if (Session.IsLogoutRequested)
                    {
                        Session.Clear();
                        Console.WriteLine("\nYou have been logged out. Returning to Home Screen...");
                        await Task.Delay(1000);
                        continue;
                    }

                    if (!exit)
                    {
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    }
                }
                catch (LogoutException)
                {
                    Session.ProcessLogout();
                    Console.WriteLine("\nYou have been logged out. Returning to Home Screen...");
                    await Task.Delay(1000);
                    continue;
                }
            }

            Console.WriteLine("Thank you for using the News Aggregator. Goodbye!");
        }
    }
}
