using news_aggregator.console.Services.Interfaces;
using System;
using System.Threading.Tasks;
using news_aggregator.console.Menu.Handler.Interface;
using news_aggregator.console.Models;

namespace news_aggregator.console.Menu.Helper
{
    public static class HeadlinesHelper
    {
        private const int MaxPreviewLength = 250;

        public static async Task ShowCategoriesAndArticlesAsync(
            string userName,
            INewsService newsService,
            ICategoryService categoryService,
            DateTime startDate,
            DateTime endDate)
        {
            Console.Clear();
            PrintWelcomeHeader(userName);

            try
            {
                var categories = await categoryService.GetAllCategoriesAsync();

                if (categories == null || categories.Count == 0)
                {
                    Console.WriteLine("No categories found.");
                    Console.ReadKey();
                    return;
                }

                DisplayCategoryOptions(categories);

                if (!TryGetSelectedCategory(categories, out string selectedCategory))
                {
                    Console.WriteLine("Invalid category selection.");
                    Console.ReadKey();
                    return;
                }

                await ShowArticlesByCategoryAsync(userName, newsService, selectedCategory, startDate, endDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to go back...");
            Console.ReadKey();
        }

        private static void PrintWelcomeHeader(string userName)
        {
            Console.WriteLine($"Welcome to the News Application, {userName}! Date: {DateTime.Today:dd-MMM-yyyy}");
            Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("Please choose the category below for Headlines:");
        }

        private static void DisplayCategoryOptions(IList<CategoryDto> categories)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Name}");
            }
        }

        private static bool TryGetSelectedCategory(IList<CategoryDto> categories, out string selectedCategory)
        {
            selectedCategory = string.Empty;
            Console.Write("Enter category number: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedIndex) || selectedIndex < 1 || selectedIndex > categories.Count)
            {
                return false;
            }

            selectedCategory = categories[selectedIndex - 1].Name;
            return true;
        }

        private static async Task ShowArticlesByCategoryAsync(
            string userName,
            INewsService newsService,
            string category,
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var articles = await newsService.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);

                Console.Clear();
                PrintHeadlinesHeader(userName);

                if (articles == null || articles.Count == 0)
                {
                    Console.WriteLine("No news articles found.");
                    return;
                }

                foreach (var article in articles)
                {
                    DisplayArticleSummary(article);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load articles: {ex.Message}");
            }
        }

        private static void PrintHeadlinesHeader(string userName)
        {
            Console.WriteLine($"Welcome to the News Application, {userName}! Date: {DateTime.Today:dd-MMM-yyyy}");
            Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("H E A D L I N E S");
            Console.WriteLine("1. Back");
            Console.WriteLine("2. Logout");
            Console.WriteLine("3. Save Article\n");
        }

        private static void DisplayArticleSummary(NewsArticleDto article)
        {
            Console.WriteLine($"Article Id: {article.NewsArticleId}");
            Console.WriteLine(article.Title);
            Console.WriteLine();

            string content = string.IsNullOrWhiteSpace(article.Content)
                ? "No content available."
                : article.Content.Length > MaxPreviewLength
                    ? article.Content.Substring(0, MaxPreviewLength) + "..."
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
