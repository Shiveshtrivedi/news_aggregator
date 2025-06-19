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
    public class UserMenu : IMenu
    {
        private readonly string _userName;
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ISavedArticleService _savedArticleService;
        private readonly ISearchArticleService _searchArticleService;
        private readonly INotificationService _notificationService;

        public UserMenu(string userName,INewsService newsService, ICategoryService categoryService, ISavedArticleService savedArticleService, ISearchArticleService searchArticleService, INotificationService notificationService)
        {
            _userName = userName;
            _newsService = newsService;
            _categoryService = categoryService;
            _savedArticleService = savedArticleService;
            _searchArticleService = searchArticleService;
            _notificationService = notificationService;
        }
        public async Task Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome to the News Application, {_userName}! Date: 22-Mar-2025");
                Console.WriteLine($"Time: 01:56PM");
                Console.WriteLine("Please choose the options below:");
                Console.WriteLine("1. Headlines");
                Console.WriteLine("2. Saved Articles");
                Console.WriteLine("3. Search");
                Console.WriteLine("4. Notifications");
                Console.WriteLine("5. Logout");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await new HeadlinesMenu(_userName,_newsService,_categoryService,_savedArticleService).Show();
                        break;
                    case "2":
                        await new SaveArticleMenu(_savedArticleService, _userName).Show();
                        break;
                    case "3":
                        await new SearchArticleMenu(_newsService, _savedArticleService,_searchArticleService,_userName).Show();
                        break;
                    case "4":
                        await new NotificationMenu(_notificationService, _userName).Show();
                        break;
                    case "5":
                        Session.Logout();
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
    }
    }
}
