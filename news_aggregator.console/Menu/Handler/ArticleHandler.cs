using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
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
            List<NewsArticleDto> articles;

            try
            {
                articles = await _newsService.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);
            }
            catch (NewsServiceException ex)
            {
                Console.WriteLine($"Error while fetching articles: {ex.Message}");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {DateTime.Today:dd-MMM-yyyy} Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("H E A D L I N E S");
            Console.WriteLine("1. Back");
            Console.WriteLine("2. Logout");
            Console.WriteLine("3. Save Article");
            Console.WriteLine("4. Like Article");
            Console.WriteLine("5. Dislike Article");
            Console.WriteLine("6. Report Article");

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
                    Console.WriteLine($"Liked by You : {(article.IsLikedByUser ? "Yes" : "No")}");
                    Console.WriteLine($"Disliked by You : {(article.IsDislikedByUser ? "Yes" : "No")}");
                    Console.WriteLine($"Likes: {article.Likes} | Dislikes: {article.Dislikes}");
                    Console.WriteLine(new string('-', 50));
                }
            }

            while (true)
            {
                Console.Write("\nChoose an option (1–6): ");
                string option = Console.ReadLine()!;

                try
                {
                    if (option == "1") return;

                    if (option == "2")
                    {
                        Console.WriteLine("Logging out...");
                        Session.Logout();
                        throw new LogoutException();
                    }

                    if (option == "3")
                    {
                        Console.Write("Enter Article Id to save: ");
                        if (int.TryParse(Console.ReadLine(), out int articleId))
                        {
                            var result = await _savedArticleService.SaveArticleAsync(Session.UserId, articleId);
                            Console.WriteLine(result ? $"Article {articleId} saved." : $"Failed to save article {articleId}.");
                        }
                        else Console.WriteLine("Invalid Article Id.");
                    }
                    else if (option == "4")
                    {
                        Console.Write("Enter Article Id to like: ");
                        if (int.TryParse(Console.ReadLine(), out int articleId))
                        {
                            var result = await _newsService.LikeArticleAsync(articleId);
                            Console.WriteLine(result ? "You liked the article." : "Failed to like article.");
                        }
                        else Console.WriteLine("Invalid Article Id.");
                    }
                    else if (option == "5")
                    {
                        Console.Write("Enter Article Id to dislike: ");
                        if (int.TryParse(Console.ReadLine(), out int articleId))
                        {
                            var result = await _newsService.DislikeArticleAsync(articleId);
                            Console.WriteLine(result ? "You disliked the article." : "Failed to dislike article.");
                        }
                        else Console.WriteLine("Invalid Article Id.");
                    }
                    else if (option == "6")
                    {
                        Console.Write("Enter Article Id to report: ");
                        if (int.TryParse(Console.ReadLine(), out int articleId))
                        {
                            Console.Write("Enter message: ");
                            string message = Console.ReadLine()!;
                            var result = await _newsService.ReportArticleAsync(articleId, message);
                            Console.WriteLine(result ? "Article reported." : "You already reported this article.");
                        }
                        else Console.WriteLine("Invalid Article Id.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid option.");
                    }
                }
                catch (LogoutException)
                {
                    throw;
                }
                catch (NewsServiceException ex)
                {
                    Console.WriteLine($"Action failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
                
            }
        }
    }
}
