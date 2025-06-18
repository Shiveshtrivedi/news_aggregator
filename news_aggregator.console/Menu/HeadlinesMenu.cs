using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Services.Interfaces;
using news_aggregator.console.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using news_aggregator.console.Http;

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
                    await ShowCategoriesAndArticlesAsync(today, today);
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

                    await ShowCategoriesAndArticlesAsync(start, end);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Logging out...");
                    Environment.Exit(0);
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Press any key...");
                    Console.ReadKey();
                }
            }
        }

        private async Task ShowCategoriesAndArticlesAsync(DateTime startDate, DateTime endDate)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {DateTime.Today:dd-MMM-yyyy}");
            Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("Please choose the category below for Headlines:");

            var categories = await _categoryService.GetAllCategoriesAsync();

            if (categories == null || categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Name}");
            }

            Console.Write("Enter category number: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedIndex) || selectedIndex < 1 || selectedIndex > categories.Count)
            {
                Console.WriteLine("Invalid category selection.");
                Console.ReadKey();
                return;
            }

            string selectedCategory = categories[selectedIndex - 1].Name;
            var articles = await _newsService.GetNewsByCategoryAndDateRangeAsync(selectedCategory, startDate, endDate);

            Console.Clear();
            Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {DateTime.Today:dd-MMM-yyyy} Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("H E A D L I N E S");
            Console.WriteLine("1. Back");
            Console.WriteLine("2. Logout");
            Console.WriteLine("3. Save Article");

            if (articles == null || articles.Count == 0)
            {
                Console.WriteLine("\nNo news articles found.");
            }
            else
            {
                foreach (var article in articles)
                {
                    Console.WriteLine($"\nArticle Id: {article.NewsArticleId}");
                    Console.WriteLine($"{article.Title}");
                    Console.WriteLine($"{article.Content?.Substring(0, Math.Min(200, article.Content.Length))}...");
                    Console.WriteLine($"source: {article.Source}");
                    Console.WriteLine($"URL: {article.Url}");
                    Console.WriteLine($"{article.Category}: {article.Category}");
                    Console.WriteLine(new string('-', 50));
                }
            }

            while (true)
            {
                Console.Write("\nChoose an option (1: Back, 2: Logout, 3: Save Article): ");
                string option = Console.ReadLine()!;

                if (option == "1")
                {
                    return;      
                }
                else if (option == "2")
                {
                    Console.WriteLine("Logging out...");
                    Environment.Exit(0);
                }
                else if (option == "3")
                {
                    Console.Write("Enter Article Id to save: ");
                    string articleIdInput = Console.ReadLine()!;
                    if (int.TryParse(articleIdInput, out int articleId))
                    {
                        var result = await _savedArticleService.SaveArticleAsync(Session.UserId, articleId);
                        Console.WriteLine(result
                            ? $"Article {articleId} saved successfully."
                            : $"Failed to save article {articleId}.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Article Id.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose 1, 2, or 3.");
                }
            }
        }
    }
}
