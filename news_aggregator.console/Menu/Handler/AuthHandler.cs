using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public class AuthHandler
    {
        private readonly IAuthService _authService;
        private readonly IServerService _serverService;
        private readonly ICategoryService _categoryService;
        private readonly INewsService _newsService;
        private readonly ISavedArticleService _savedArticleService;
        private readonly ISearchArticleService _searchArticleService;
        private readonly INotificationService _notificationService;
        private readonly IBlockedKeywordService _blockedKeywordService;
        private readonly IUserKeywordService _userKeywordService;

        public AuthHandler(
            IAuthService authService,
            IServerService serverService,
            ICategoryService categoryService,
            INewsService newsService,
            ISavedArticleService savedArticleService,
            ISearchArticleService searchArticleService,
            INotificationService notificationService,
            IBlockedKeywordService blockedKeywordService,
            IUserKeywordService userKeywordService)
        {
            _authService = authService;
            _serverService = serverService;
            _categoryService = categoryService;
            _newsService = newsService;
            _savedArticleService = savedArticleService;
            _searchArticleService = searchArticleService;
            _notificationService = notificationService;
            _blockedKeywordService = blockedKeywordService;
            _userKeywordService = userKeywordService;
        }

        public async Task HandleLoginAsync()
        {
            Console.Write("Email: ");
            string email = Console.ReadLine()!;
            Console.Write("Password: ");
            string password = ReadPassword()!;

            var user = await _authService.LoginAsync(email, password);
            if (user == null)
            {
                Console.WriteLine("Login failed.");
                return;
            }

            if (user.Role == 1)
            {
                var adminMenu = new AdminMenu(user.UserName, _serverService, _categoryService,_blockedKeywordService);
                await adminMenu.Show();
            }
            else
            {
                var userMenu = new UserMenu(user.UserName, _newsService, _categoryService, _savedArticleService, _searchArticleService, _notificationService,_userKeywordService);
                await userMenu.Show();
            }
        }

        public async Task HandleSignUpAsync()
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

            if (success)
            {
                Console.WriteLine("Redirecting to login...\n");
                Console.Clear();
                await HandleLoginAsync();
            }
        }

        private static string ReadPassword()
        {
            StringBuilder passwordBuilder = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Backspace && passwordBuilder.Length > 0)
                {
                    Console.Write("\b \b");     
                    passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    passwordBuilder.Append(keyInfo.KeyChar);
                    Console.Write("*");       
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();       
            return passwordBuilder.ToString();
        }

    }
}
