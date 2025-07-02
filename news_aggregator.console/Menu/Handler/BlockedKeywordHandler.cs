using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public class BlockedKeywordHandler
    {
        private readonly IBlockedKeywordService _blockedKeywordService;

        public BlockedKeywordHandler(IBlockedKeywordService blockedKeywordService)
        {
            _blockedKeywordService = blockedKeywordService;
        }

        public async Task ManageBlockedKeywordsAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Blocked Keyword Management ===");
                Console.WriteLine("1. View Blocked Keywords");
                Console.WriteLine("2. Add a Blocked Keyword");
                Console.WriteLine("3. Remove a Blocked Keyword");
                Console.WriteLine("4. Back");

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var keywords = await _blockedKeywordService.GetAllBlockedKeywordsAsync();
                        if (keywords.Count == 0)
                            Console.WriteLine("No keywords found.");
                        else
                        {
                            Console.WriteLine("Blocked Keywords:");
                            keywords.ForEach(k => Console.WriteLine($"- {k}"));
                        }
                        break;

                    case "2":
                        Console.Write("Enter keyword to block: ");
                        var newKeyword = Console.ReadLine();
                        await _blockedKeywordService.AddBlockedKeywordAsync(newKeyword);
                        Console.WriteLine("Keyword blocked successfully.");
                        break;

                    case "3":
                        Console.Write("Enter keyword to unblock: ");
                        var keywordToRemove = Console.ReadLine();
                        await _blockedKeywordService.RemoveBlockedKeywordAsync(keywordToRemove);
                        Console.WriteLine("Keyword removed successfully.");
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
