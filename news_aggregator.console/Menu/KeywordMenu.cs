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
    public class KeywordMenu : IMenu
    {
        private readonly IUserKeywordService _userKeywordService;
        private readonly string _userName;
        private readonly int _userId;

        public KeywordMenu(IUserKeywordService userKeywordService, string userName, int userId)
        {
            _userKeywordService = userKeywordService;
            _userName = userName;
            _userId = userId;
        }

        public async Task Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Keyword Preferences - {_userName}");
                Console.WriteLine("1. Add Keywords (overwrite existing)");
                Console.WriteLine("2. View My Keywords");
                Console.WriteLine("3. Back");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter keywords separated by commas: ");
                        var input = Console.ReadLine() ?? "";
                        var keywords = input
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(k => k.Trim())
                            .Where(k => !string.IsNullOrWhiteSpace(k))
                            .ToList();

                        await _userKeywordService.SetKeywordsAsync(_userId, keywords);
                        Console.WriteLine("✅ Keywords updated successfully.");
                        break;

                    case "2":
                        var userKeywords = await _userKeywordService.GetKeywordsAsync();
                        Console.WriteLine("\nYour current keywords:");
                        if (!userKeywords.Any())
                            Console.WriteLine("⚠️  No keywords set.");
                        else
                            foreach (var keyword in userKeywords)
                                Console.WriteLine($"- {keyword}");
                        break;

                    case "3":
                        return;

                    default:
                        Console.WriteLine("❌ Invalid choice.");
                        break;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
