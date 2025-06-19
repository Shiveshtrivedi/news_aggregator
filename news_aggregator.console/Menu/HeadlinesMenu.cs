using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Services.Interfaces;
using news_aggregator.console.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using news_aggregator.console.Http;
using news_aggregator.console.Menu.Handler;

namespace news_aggregator.console.Menu
{
    public class HeadlinesMenu : IMenu
    {
        private readonly string _userName;
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ISavedArticleService _savedArticleService;

        public HeadlinesMenu(string userName, INewsService newsService, ICategoryService categoryService, ISavedArticleService savedArticleService)
        {
            _userName = userName;
            _newsService = newsService;
            _categoryService = categoryService;
            _savedArticleService = savedArticleService;
        }

        public async Task Show()
        {
            var categorySelector = new CategorySelector(_categoryService);
            var articleHandler = new ArticleHandler(_userName, _newsService, _savedArticleService);

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {DateTime.Today:dd-MMM-yyyy}");
                Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
                Console.WriteLine("Please choose the options below:");
                Console.WriteLine("1. Today");
                Console.WriteLine("2. Date Range");
                Console.WriteLine("3. Logout");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine()!;

                if (choice == "1")
                {
                    DateTime today = DateTime.Today;
                    await ShowCategoriesAndArticlesAsync(today, today, categorySelector, articleHandler);
                }
                else if (choice == "2")
                {
                    Console.Write("Enter Start Date (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime start))
                    {
                        Console.WriteLine("Invalid start date.");
                        Console.ReadKey();
                        continue;
                    }

                    Console.Write("Enter End Date (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime end))
                    {
                        Console.WriteLine("Invalid end date.");
                        Console.ReadKey();
                        continue;
                    }

                    if (start > end)
                    {
                        Console.WriteLine("Start date must be before or equal to end date.");
                        Console.ReadKey();
                        continue;
                    }

                    await ShowCategoriesAndArticlesAsync(start, end, categorySelector, articleHandler);
                }
                else if (choice == "3")
                {
                    Session.Logout();
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Press any key...");
                    Console.ReadKey();
                }
            }
        }

        private async Task ShowCategoriesAndArticlesAsync(DateTime startDate, DateTime endDate, CategorySelector selector, ArticleHandler handler)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {DateTime.Today:dd-MMM-yyyy}");
            Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("Please choose the category below for Headlines:");

            var category = await selector.SelectCategoryAsync();
            if (category == null) return;

            await handler.ShowAndHandleArticlesAsync(category, startDate, endDate);
        }
    }
}
