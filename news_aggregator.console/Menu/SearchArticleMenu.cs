using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Services.Interfaces;

namespace news_aggregator.console.Menu
{
    public class SearchArticleMenu : IMenu
    {
        private readonly INewsService _newsService;
        private readonly ISavedArticleService _savedArticleService;
        private readonly ISearchArticleService _searchArticleService;
        private readonly string _userName;

        public SearchArticleMenu(INewsService newsService, ISavedArticleService savedArticleService, ISearchArticleService searchArticleService, string userName)
        {
            _newsService = newsService;
            _savedArticleService = savedArticleService;
            _searchArticleService = searchArticleService;
            _userName = userName;
        }

        public async Task Show()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the News Application, {_userName}!");
            Console.WriteLine($"Date: {DateTime.Today:dd-MMM-yyyy} Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("S E A R C H\n");

            Console.Write("Enter search query: ");
            string query = Console.ReadLine() ?? "";

            Console.Write("Enter start date (yyyy-MM-dd) or press Enter to skip: ");
            string startDateInput = Console.ReadLine();
            DateTime? startDate = DateTime.TryParse(startDateInput, out DateTime tempStart) ? tempStart : null;

            Console.Write("Enter end date (yyyy-MM-dd) or press Enter to skip: ");
            string endDateInput = Console.ReadLine();
            DateTime? endDate = DateTime.TryParse(endDateInput, out DateTime tempEnd) ? tempEnd : null;

            var articles = await _searchArticleService.SearchArticlesAsync(query, startDate, endDate);

            Console.Clear();
            Console.WriteLine($"Welcome to the News Application, {_userName}!");
            Console.WriteLine($"Date: {DateTime.Today:dd-MMM-yyyy} Time: {DateTime.Now:hh:mmtt}");
            Console.WriteLine("S E A R C H\n");
            Console.WriteLine($"Results for “{query}”\n");

            DisplayMenu();

            foreach (var article in articles)
            {
                Console.WriteLine($"\nArticle Id: {article.NewsArticleId} {article.Title}");
                Console.WriteLine(article.Content);
                Console.WriteLine($"source : {article.Source}");
                Console.WriteLine($"URL: {article.Url}");
                Console.WriteLine($"Category: {article.Category}");
            }

            while (true)
            {
                Console.Write("\nChoose an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        return;
                    case "2":
                        Session.Logout();
                        throw new LogoutException();
                    case "3":
                        Console.Write("Enter Article Id to save: ");
                        if (int.TryParse(Console.ReadLine(), out int articleId))
                        {
                            await _savedArticleService.SaveArticleAsync(articleId, Session.UserId);
                            Console.WriteLine("Article saved successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Article Id.");
                        }
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("1. Back");
            Console.WriteLine("2. Logout");
            Console.WriteLine("3. Save Article");
        }


    }
}
