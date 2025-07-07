using news_aggregator.console.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public static class HeadlinesHelper
    {
        public static async Task ShowCategoriesAndArticlesAsync(
            string userName,
            INewsService newsService,
            ICategoryService categoryService,
            DateTime startDate,
            DateTime endDate)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the News Application, {userName}! Date: {DateTime.Today:dd-MMM-yyyy}");
            Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("Please choose the category below for Headlines:");

            try
            {
                var categories = await categoryService.GetAllCategoriesAsync();

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

                try
                {
                    var articles = await newsService.GetNewsByCategoryAndDateRangeAsync(selectedCategory, startDate, endDate);

                    Console.Clear();
                    Console.WriteLine($"Welcome to the News Application, {userName}! Date: {DateTime.Today:dd-MMM-yyyy}");
                    Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
                    Console.WriteLine("H E A D L I N E S");
                    Console.WriteLine("1. Back");
                    Console.WriteLine("2. Logout");
                    Console.WriteLine("3. Save Article\n");

                    if (articles == null || articles.Count == 0)
                    {
                        Console.WriteLine("No news articles found.");
                    }
                    else
                    {
                        foreach (var article in articles)
                        {
                            Console.WriteLine($"Article Id: {article.NewsArticleId}");
                            Console.WriteLine(article.Title);
                            Console.WriteLine();

                            string content = string.IsNullOrWhiteSpace(article.Content)
                                ? "No content available."
                                : article.Content.Length > 250
                                    ? article.Content.Substring(0, 250) + "..."
                                    : article.Content;

                            Console.WriteLine(content);
                            Console.WriteLine();
                            Console.WriteLine($"Source: {article.Source ?? "Unknown"}");
                            Console.WriteLine($"URL: {article.Url ?? "N/A"}");
                            Console.WriteLine($"Category: {article.Category}");
                            Console.WriteLine(new string('-', 60));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load articles: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to go back...");
            Console.ReadKey();
        }
    }
}
