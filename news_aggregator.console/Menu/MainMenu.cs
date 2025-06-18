using Microsoft.Extensions.Configuration;
using news_aggregator.console.Http;
using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Models;
using news_aggregator.console.Services;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public MainMenu(IAuthService authService, IServerService serverService, ICategoryService categoryService, INewsService newsService, ISavedArticleService savedArticleService, ISearchArticleService searchArticleService, INotificationService notificationService)
        {
            _authService = authService;
            _serverService = serverService;
            _categoryService = categoryService;
            _newsService = newsService;
            _savedArticleService = savedArticleService;
            _searchArticleService = searchArticleService;
            _notificationService = notificationService;
        }

        private async Task HandleLoginAsync()
        {
            Console.Write("Email: ");
            string email = Console.ReadLine()!;
            Console.Write("Password: ");
            string password = Console.ReadLine()!;

            var user = await _authService.LoginAsync(email, password);
            if (user == null)
            {
                Console.WriteLine("Login failed.");
                return;
            }

            if (user.Role == 1)
            {
                var adminMenu = new AdminMenu(user.UserName, _serverService, _categoryService);
                await adminMenu.Show();
            }
            else
            {
                var userMenu = new UserMenu(user.UserName, _newsService,_categoryService, _savedArticleService,_searchArticleService,_notificationService);
                await userMenu.Show();
            }
        }

        private async Task HandleSignUpAsync()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine()!;
            Console.Write("Email: ");
            string email = Console.ReadLine()!;
            Console.Write("Password: ");
            string password = Console.ReadLine()!;


            var userDto = new UserDto
            {
                UserName = name,
                Email = email,
                Password = password
            };
            var success = await _authService.SignUpAsync(userDto);
            Console.WriteLine(success ? "Sign-up successful." : "Sign-up failed.");

            if(success)
            {
                Console.WriteLine("Redirecting to login...\n");
                Console.Clear();
                await HandleLoginAsync();
            }

        }
        public async Task Show()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                                                        .AddJsonFile("appsettings.json")
                                                        .Build();

            IHttpClientFactoryWrapper clientFactoryWrapper = new HttpClientFactory(configuration);

            Console.Clear();
            Console.WriteLine("Welcome to the News Aggregator application. Please choose the options below.");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Sign up");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();
            IAuthService authService = new AuthService(clientFactoryWrapper);

            switch (choice)
            {
                case "1":
                    await HandleLoginAsync();
                    break;
                case "2":
                    await HandleSignUpAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
