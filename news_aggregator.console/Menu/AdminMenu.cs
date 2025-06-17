using news_aggregator.console.Menu.Interfaces;
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
        private readonly IServerService _serverService;
        private readonly ICategoryService _categoryService;

        public AdminMenu(string userName, IServerService serverService, ICategoryService categoryService)
        {
            _userName = userName;
            _serverService = serverService;
            _categoryService = categoryService;
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
                Console.WriteLine("5. Logout");

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ShowExternalServerStatuses();
                        break;
                    case "2":
                        await ShowExternalServerDetails();
                        break;
                    case "3":
                        await UpdateExternalServer();
                        break;
                    case "4":
                        await AddNewCategory();
                        break;
                    case "5":
                        return; 
                    default:
                        Console.WriteLine("Invalid choice. Press any key...");
                        Console.ReadKey();
                        break;
                }

            }

        }
        private async Task ShowExternalServerStatuses()
        {
            var statuses = await _serverService.GetServerStatusesAsync();
            Console.WriteLine("\nList of external servers:");
            foreach (var server in statuses)
            {
                Console.WriteLine($"{server.ExternalSourceName} - {server.IsActive} - {server.LastAccessed:dd MMM yyyy}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task ShowExternalServerDetails()
        {
            var statuses = await _serverService.GetServerStatusesAsync();
            Console.WriteLine("\nList of external servers:");
            foreach (var server in statuses)
            {
                Console.WriteLine($"{server.ExternalSourceName} - {server.ApiKey}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        private async Task UpdateExternalServer()
        {
            Console.Write("Enter Server ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int serverId))
            {
                Console.Write("Enter updated API Key: ");
                string updatedKey = Console.ReadLine()!;
                var updateDto = new ServerUpdateDto
                {
                    ApiKey = updatedKey
                };

                var result = await _serverService.UpdateServerAsync(serverId, updateDto);
                Console.WriteLine(result ? "Server updated successfully." : "Failed to update.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task AddNewCategory()
        {
            Console.Write("Enter new category name: ");
            var name = Console.ReadLine();
            var success = await _categoryService.AddCategoryAsync(name!); 
            Console.WriteLine(success ? "Category added successfully." : "Failed to add category.");
            Console.ReadKey();
        }


    }
}
