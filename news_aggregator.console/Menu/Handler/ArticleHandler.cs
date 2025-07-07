using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using news_aggregator.console.Menu.Handler.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using news_aggregator.console.Menu.Helper.Interface;

namespace news_aggregator.console.Menu.Handler
{
    public class ArticleHandler : IArticleHandler
    {
        private readonly INewsService _newsService;
        private readonly ISavedArticleService _savedArticleService;
        private readonly string _userName;
        private readonly IArticleActionHelper _actionHelper;

        public ArticleHandler(string userName, INewsService newsService, ISavedArticleService savedArticleService)
        {
            _userName = userName;
            _newsService = newsService;
            _savedArticleService = savedArticleService;
            _actionHelper = new ArticleActionHelper(newsService, savedArticleService);
        }

        public async Task ShowAndHandleArticlesAsync(string category, DateTime startDate, DateTime endDate)
        {
            var articles = await FetchArticlesAsync(category, startDate, endDate);
            if (articles == null) return;

            DisplayHeader();
            DisplayArticles(articles);

            await HandleUserActionsAsync(articles);
        }

        private async Task<List<NewsArticleDto>?> FetchArticlesAsync(string category, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _newsService.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);
            }
            catch (NewsServiceException ex)
            {
                Console.WriteLine($"Error while fetching articles: {ex.Message}");
                Console.ReadKey();
                return null;
            }
        }

        private void DisplayHeader()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {DateTime.Today:dd-MMM-yyyy} Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("H E A D L I N E S");
            Console.WriteLine("1. Back");
            Console.WriteLine("2. Logout");
            Console.WriteLine("3. Save Article");
            Console.WriteLine("4. Like Article");
            Console.WriteLine("5. Dislike Article");
            Console.WriteLine("6. Report Article");
        }

        private void DisplayArticles(List<NewsArticleDto> articles)
        {
            if (articles == null || articles.Count == 0)
            {
                Console.WriteLine("\nNo news articles found.");
                return;
            }

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

        private async Task HandleUserActionsAsync(List<NewsArticleDto> articles)
        {
            while (true)
            {
                Console.Write("\nChoose an option (1–6): ");
                string option = Console.ReadLine()!;

                try
                {
                    switch (option)
                    {
                        case "1":
                            return;
                        case "2":
                            Console.WriteLine("Logging out...");
                            Session.Logout();
                            throw new LogoutException();
                        case "3":
                            await _actionHelper.HandleSaveArticleAsync();
                            break;
                        case "4":
                            await _actionHelper.HandleLikeArticleAsync();
                            break;
                        case "5":
                            await _actionHelper.HandleDislikeArticleAsync();
                            break;
                        case "6":
                            await _actionHelper.HandleReportArticleAsync();
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
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
