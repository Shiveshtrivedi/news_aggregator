using news_aggregator.console.Menu.Handler.Interface;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public class BlockedKeywordHandler : IBlockedKeywordHandler
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
                        await ViewBlockedKeywordsAsync();
                        break;

                    case "2":
                        await AddBlockedKeywordAsync();
                        break;

                    case "3":
                        await RemoveBlockedKeywordAsync();
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

        private async Task ViewBlockedKeywordsAsync()
        {
            try
            {
                var keywords = await _blockedKeywordService.GetAllBlockedKeywordsAsync();

                if (keywords.Count == 0)
                    Console.WriteLine("No keywords found.");
                else
                {
                    Console.WriteLine("Blocked Keywords:");
                    keywords.ForEach(k => Console.WriteLine($"- {k}"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching blocked keywords: {ex.Message}");
            }
        }

        private async Task AddBlockedKeywordAsync()
        {
            Console.Write("Enter keyword to block: ");
            var newKeyword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newKeyword))
            {
                Console.WriteLine("Keyword cannot be empty.");
                return;
            }

            try
            {
                await _blockedKeywordService.AddBlockedKeywordAsync(newKeyword);
                Console.WriteLine("Keyword blocked successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add keyword: {ex.Message}");
            }
        }

        private async Task RemoveBlockedKeywordAsync()
        {
            Console.Write("Enter keyword to unblock: ");
            var keywordToRemove = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(keywordToRemove))
            {
                Console.WriteLine("Keyword cannot be empty.");
                return;
            }

            try
            {
                await _blockedKeywordService.RemoveBlockedKeywordAsync(keywordToRemove);
                Console.WriteLine("Keyword removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remove keyword: {ex.Message}");
            }
        }
    }
}
