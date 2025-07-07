using news_aggregator.console.Exceptions;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.NewFolder
{
    public class ExternalServerHandler
    {
        private readonly IServerService _serverService;

        public ExternalServerHandler(IServerService serverService)
        {
            _serverService = serverService;
        }

        public async Task ShowExternalServerStatuses()
        {
            try
            {
                var statuses = await _serverService.GetServerStatusesAsync();
                Console.WriteLine("\nList of external servers:");
                foreach (var server in statuses)
                {
                    Console.WriteLine($"{server.ExternalSourceName} - {server.IsActive} - {server.LastAccessed:dd MMM yyyy}");
                }
            }
            catch (ServerServiceException ex)
            {
                Console.WriteLine($"Failed to fetch server statuses: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public async Task ShowExternalServerDetails()
        {
            try
            {
                var statuses = await _serverService.GetServerStatusesAsync();
                Console.WriteLine("\nList of external servers:");
                foreach (var server in statuses)
                {
                    Console.WriteLine($"{server.ExternalSourceName} - {server.ApiKey}");
                }
            }
            catch (ServerServiceException ex)
            {
                Console.WriteLine($"Failed to fetch server details: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public async Task UpdateExternalServer()
        {
            Console.Write("Enter Server ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int serverId))
            {
                Console.Write("Enter updated API Key: ");
                string apiKey = Console.ReadLine()!;

                Console.Write("Enter updated Base URL: ");
                string baseUrl = Console.ReadLine()!;

                Console.Write("Enter updated Auth Param Name: ");
                string authParamName = Console.ReadLine()!;

                Console.Write("Enter updated Auth Location (e.g., header/query): ");
                string authLocation = Console.ReadLine()!;

                Console.Write("Is the server active? (true/false): ");
                if (!bool.TryParse(Console.ReadLine(), out bool isActive))
                {
                    Console.WriteLine("Invalid value for IsActive. Please enter true or false.");
                    return;
                }

                var updateDto = new ServerUpdateDto
                {
                    Id = serverId,
                    ApiKey = apiKey,
                    BaseUrl = baseUrl,
                    AuthParamName = authParamName,
                    AuthLocation = authLocation,
                    IsActive = isActive
                };

                try
                {
                    var result = await _serverService.UpdateServerAsync(serverId, updateDto);
                    Console.WriteLine(result ? "Server updated successfully." : "Failed to update.");
                }
                catch (ServerServiceException ex)
                {
                    Console.WriteLine($"Failed to update server: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error while updating: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
