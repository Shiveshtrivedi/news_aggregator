using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;

namespace news_aggregator.console.Menu
{
    public class SaveArticleMenu : IMenu
    {
        private readonly ISavedArticleService _savedArticleService;
        private readonly string _userName;

        public SaveArticleMenu(ISavedArticleService savedArticleService, string userName)
        {
            _savedArticleService = savedArticleService;
            _userName = userName;
        }

        public async Task Show()
        {
            while (true)
            {
                Console.Clear();
                DisplayHeader();
                DisplayMenu();
                var savedArticles = await _savedArticleService.GetSavedArticlesAsync(Session.UserId);
                DisplayArticles(savedArticles);

                Console.Write("Choose an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        return;

                    case "2":
                        Session.Logout();
                        throw new LogoutException();

                    case "3":
                        Console.Write("Enter the Article Id to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            await _savedArticleService.DeleteArticleAsync(id, Session.UserId);
                            Console.WriteLine("Article deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Article Id. Please enter a valid number.");
                        }

                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void DisplayHeader()
        {
            Console.WriteLine($"Welcome to the News Application, {_userName}!");
            Console.WriteLine($"Date: {DateTime.Today:dd-MMM-yyyy}");
            Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}\n");
            Console.WriteLine("\"H E A D L I N E S\"\n");
        }

        private void DisplayArticles(IEnumerable<NewsArticleDto> articles)
        {
            foreach (var article in articles)
            {
                Console.WriteLine($"Article Id: {article.NewsArticleId} {article.Title}");
                Console.WriteLine(article.Content);
                Console.WriteLine($"source : {article.Source}");
                Console.WriteLine("URL:");
                Console.WriteLine(article.Url);
                Console.WriteLine($"Category: {article.Category}\n");
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("1. Back");
            Console.WriteLine("2. Logout");
            Console.WriteLine("3. Delete Article");
        }
    }
}
