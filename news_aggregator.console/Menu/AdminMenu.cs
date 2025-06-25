using news_aggregator.console.Http;
using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Menu.NewFolder;
using news_aggregator.console.Models;
using news_aggregator.console.Services;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu
{
    public class AdminMenu : IMenu
    {
        private readonly string _userName;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly ExternalServerHandler _serverHandler;
        private readonly CategoryHandler _categoryHandler;

        public AdminMenu(string userName, IServerService serverService, ICategoryService categoryService)
        {
            _userName = userName;
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
            _serverHandler = new ExternalServerHandler(serverService);
            _categoryHandler = new CategoryHandler(categoryService);
        }

        public async Task Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome to the News Application, {_userName}! Date: {_startDate:dd-MMM-yyyy}");
                Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
                Console.WriteLine("Please choose the options below for Headlines:");
                Console.WriteLine("1. View the list of external servers and status");
                Console.WriteLine("2. View the external server’s details");
                Console.WriteLine("3. Update/Edit the external server’s details");
                Console.WriteLine("4. Add new News Category");
                Console.WriteLine("5. Hide/Unhide News Category");
                Console.WriteLine("6. Logout");

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await _serverHandler.ShowExternalServerStatuses();
                        break;
                    case "2":
                        await _serverHandler.ShowExternalServerDetails();
                        break;
                    case "3":
                        await _serverHandler.UpdateExternalServer();
                        break;
                    case "4":
                        await _categoryHandler.AddNewCategory();
                        break;
                    case "5":
                        await _categoryHandler.ToggleCategoryvisibility();
                        break;
                    case "6":
                        Session.Logout();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

    }
}
        