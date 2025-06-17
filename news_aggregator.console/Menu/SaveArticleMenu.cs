//using news_aggregator.console.Menu.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace news_aggregator.console.Menu
//{
//    public class SaveArticleMenu : IMenu
//    {
//        private readonly ISavedArticleService _savedArticleService;
//        private readonly string _userName;

//        public SaveArticleMenu(ISavedArticleService savedArticleService, string userName)
//        {
//            _savedArticleService = savedArticleService;
//            _userName = userName;
//        }

//        public async Task Show()
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {DateTime.Now:dd-MMM-yyyy}");
//                Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
//                Console.WriteLine("S A V E D");
//                Console.WriteLine("1. View Saved Articles");
//                Console.WriteLine("2. Delete Article");
//                Console.WriteLine("3. Back");
//                Console.WriteLine("4. Logout");

//                var option = Console.ReadLine();

//                switch (option)
//                {
//                    case "1":
//                        var articles = await _savedArticleService.GetSavedArticlesAsync(_userName);
//                        foreach (var article in articles)
//                        {
//                            Console.WriteLine($"Article Id: {article.Id} {article.Title}");
//                            Console.WriteLine(article.Description);
//                            Console.WriteLine($"source : {article.Source}");
//                            Console.WriteLine($"URL: {article.Url}");
//                            Console.WriteLine($"Category: {article.Category}");
//                            Console.WriteLine("----------------------------------------------");
//                        }
//                        Console.WriteLine("Press Enter to continue...");
//                        Console.ReadLine();
//                        break;

//                    case "2":
//                        Console.Write("Enter Article Id to delete: ");
//                        var idToDelete = Console.ReadLine();
//                        await _savedArticleService.DeleteArticleAsync(idToDelete, _userName);
//                        Console.WriteLine("Deleted successfully.");
//                        Console.ReadLine();
//                        break;

//                    case "3":
//                        return;

//                    case "4":
//                        Environment.Exit(0);
//                        break;

//                    default:
//                        Console.WriteLine("Invalid option");
//                        break;
//                }
//            }
//        }
//    }
//}
