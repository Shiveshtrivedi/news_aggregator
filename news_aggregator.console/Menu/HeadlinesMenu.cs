using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu
{
    public class HeadlinesMenu : IMenu
    {
        private readonly string _userName;
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;


        public HeadlinesMenu(string userName, INewsService newsService, ICategoryService categoryService)
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
            Console.WriteLine($"News for {selectedCategory} from {startDate:dd-MMM-yyyy} to {endDate:dd-MMM-yyyy}");

            if (articles == null || articles.Count == 0)
            {
                Console.WriteLine("No news articles found.");
            }
            else
            {
                foreach (var article in articles)
                {
                    Console.WriteLine($"\nTitle: {article.Title}");
                    Console.WriteLine($"Published: {article.PublishedAt}");
                    Console.WriteLine($"Source: {article.Source}");
                    Console.WriteLine($"URL: {article.Url}");
                    Console.WriteLine(new string('-', 40));
                }
            }

            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
        }
    }
}
