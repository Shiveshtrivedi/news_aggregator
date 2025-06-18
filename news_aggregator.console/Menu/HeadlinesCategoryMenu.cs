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
    public class HeadlinesCategoryMenu : IMenu
    {
        private readonly string _userName;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsService;
        private readonly ISavedArticleService _savedArticleService;

        public HeadlinesCategoryMenu(string userName, DateTime startDate, DateTime endDate, ICategoryService categoryService, INewsArticleService newsService, ISavedArticleService savedArticleService)
        {
            _userName = userName;
            _startDate = startDate;
            _endDate = endDate;
            _categoryService = categoryService;
            _newsService = newsService;
            _savedArticleService = savedArticleService;
        }

        public async Task Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {_startDate:dd-MMM-yyyy}");
                Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
                Console.WriteLine("Please choose the options below for Headlines:");

                var categories = await _categoryService.GetAllCategoriesAsync();

                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i].Name}");
                }
                int backOption = categories.Count + 1;
                int logoutOption = categories.Count + 2;
                Console.WriteLine($"{backOption}. Back");
                Console.WriteLine($"{logoutOption}. Logout");

                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice) || choice < 1 || choice > logoutOption)
                {
                    Console.WriteLine("Invalid option.");
                    Console.ReadKey();
                    continue;
                }

                if (choice == backOption)
                    return;
                if (choice == logoutOption)
                    Environment.Exit(0);

                string selectedCategory = categories[choice - 1].Name;

                var articles = await _newsService.GetNewsByCategoryAndDateRangeAsync(selectedCategory, _startDate, _endDate);

                if (articles.Count == 0)
                {
                    Console.WriteLine("No articles found.");
                }
                else
                {
                    foreach (var article in articles)
                    {
                        Console.WriteLine($"\nArticle Id: {article.NewsArticleId}\n{article.Title}\n{article.Content}\nSource: {article.Category}\nCategory: {article.Category}");
                    }
                }

                Console.WriteLine("\n1. Back\n2. Save Article\n3. Logout");
                var action = Console.ReadLine();
                if (action == "2")
                {
                    Console.Write("Enter article ID to save: ");
                    var articleId = int.Parse(Console.ReadLine());
                    await _savedArticleService.SaveArticleAsync(Session.UserId, articleId);
                    Console.WriteLine("Article saved.");
                    Console.ReadKey();
                }
                else if (action == "3")
                {
                    Environment.Exit(0);
                }

            }

        }
    }
}
