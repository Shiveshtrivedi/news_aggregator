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

        public UserMenu(string userName,INewsService newsService, ICategoryService categoryService)
        {
            _userName = userName;
            _newsService = newsService;
            _categoryService = categoryService;
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
                        await new HeadlinesMenu(_userName,_newsService,_categoryService).Show();
                        break;
                    case "2":
                        //await new SavedArticlesMenu(_userName).ShowAsync();
                        break;
                    case "3":
                        //await new SearchMenu(_userName).ShowAsync();
                        break;
                    case "4":
                        //await new NotificationMenu(_userName).ShowAsync();
                        break;
                    case "5":
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
