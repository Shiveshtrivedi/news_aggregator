using news_aggregator.console.Exceptions;
using news_aggregator.console.Menu.Handler.Interface;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public class AuthHandler : IAuthHandler
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
            var (email, password) = AuthInputHelper.ReadLoginCredentials();

            try
            {
                var user = await _authService.LoginAsync(email, password);
                if (user == null)
                {
                    Console.WriteLine("Login failed.");
                    return;
                }

                await NavigateToRoleBasedMenuAsync(user);
            }
            catch (AuthServiceException ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid login state: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during login: {ex.Message}");
            }
        }

        public async Task HandleSignUpAsync()
        {
            var userDto = AuthInputHelper.ReadSignupDetails();

            try
            {
                var success = await _authService.SignUpAsync(userDto);
                Console.WriteLine(success ? "Sign-up successful." : "Sign-up failed.");

                if (success)
                {
                    Console.WriteLine("Redirecting to login...\n");
                    Console.Clear();
                    await HandleLoginAsync();
                }
            }
            catch (AuthServiceException ex)
            {
                Console.WriteLine($"Sign-up error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during sign-up: {ex.Message}");
            }
        }

        private async Task NavigateToRoleBasedMenuAsync(UserDto user)
        {
            if (user.Role == 1)
            {
                var adminMenu = new AdminMenu(user.UserName, _serverService, _categoryService, _blockedKeywordService);
                await adminMenu.Show();
            }
            else
            {
                var userMenu = new UserMenu(
                    user.UserName,
                    _newsService,
                    _categoryService,
                    _savedArticleService,
                    _searchArticleService,
                    _notificationService,
                    _userKeywordService);
                await userMenu.Show();
            }
        }
    }
}
