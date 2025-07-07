using news_aggregator.console.Http;
using news_aggregator.console.Menu.Helper.Interface;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public class ArticleActionHelper : IArticleActionHelper
    {
        private readonly INewsService _newsService;
        private readonly ISavedArticleService _savedArticleService;

        public ArticleActionHelper(INewsService newsService, ISavedArticleService savedArticleService)
        {
            _newsService = newsService;
            _savedArticleService = savedArticleService;
        }

        public async Task HandleSaveArticleAsync()
        {
            Console.Write("Enter Article Id to save: ");
            if (int.TryParse(Console.ReadLine(), out int articleId))
            {
                var result = await _savedArticleService.SaveArticleAsync(Session.UserId, articleId);
                Console.WriteLine(result ? $"Article {articleId} saved." : $"Failed to save article {articleId}.");
            }
            else
            {
                Console.WriteLine("Invalid Article Id.");
            }
        }

        public async Task HandleLikeArticleAsync()
        {
            Console.Write("Enter Article Id to like: ");
            if (int.TryParse(Console.ReadLine(), out int articleId))
            {
                var result = await _newsService.LikeArticleAsync(articleId);
                Console.WriteLine(result ? "You liked the article." : "Failed to like article.");
            }
            else
            {
                Console.WriteLine("Invalid Article Id.");
            }
        }

        public async Task HandleDislikeArticleAsync()
        {
            Console.Write("Enter Article Id to dislike: ");
            if (int.TryParse(Console.ReadLine(), out int articleId))
            {
                var result = await _newsService.DislikeArticleAsync(articleId);
                Console.WriteLine(result ? "You disliked the article." : "Failed to dislike article.");
            }
            else
            {
                Console.WriteLine("Invalid Article Id.");
            }
        }

        public async Task HandleReportArticleAsync()
        {
            Console.Write("Enter Article Id to report: ");
            if (int.TryParse(Console.ReadLine(), out int articleId))
            {
                Console.Write("Enter message: ");
                string message = Console.ReadLine()!;
                var result = await _newsService.ReportArticleAsync(articleId, message);
                Console.WriteLine(result ? "Article reported." : "You already reported this article.");
            }
            else
            {
                Console.WriteLine("Invalid Article Id.");
            }
        }
    }
}
