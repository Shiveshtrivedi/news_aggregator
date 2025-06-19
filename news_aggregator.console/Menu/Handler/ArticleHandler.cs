using news_aggregator.console.Http;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public class ArticleHandler
    {
        private readonly INewsService _newsService;
        private readonly ISavedArticleService _savedArticleService;
        private readonly string _userName;

        public ArticleHandler(string userName, INewsService newsService, ISavedArticleService savedArticleService)
        {
            _userName = userName;
            _newsService = newsService;
            _savedArticleService = savedArticleService;
        }

        public async Task ShowAndHandleArticlesAsync(string category, DateTime startDate, DateTime endDate)
        {
            var articles = await _newsService.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);

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
                    return;

                if (option == "2")
                {
                    Console.WriteLine("Logging out...");
                    Environment.Exit(0);
                }

                if (option == "3")
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
