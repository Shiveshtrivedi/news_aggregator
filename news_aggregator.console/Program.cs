using Microsoft.Extensions.Configuration;
using news_aggregator.console.Http;
using news_aggregator.console.Menu;
using news_aggregator.console.Menu.Interfaces;
using news_aggregator.console.Services;
using news_aggregator.console.Services.Interfaces;
using System.Threading.Tasks;

namespace news_aggregator.console
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();

            // Load config
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Create HttpClientFactory
            IHttpClientFactoryWrapper clientFactory = new HttpClientFactory(configuration);

            // Inject AuthService
            IAuthService authService = new AuthService(clientFactory);
            IServerService serverService = new ServerService(clientFactory);
            ICategoryService categoryService= new CategoryService(clientFactory);
            INewsService newsService = new NewsService(clientFactory);

            // Start main menu
            IMenu menu = new MainMenu(authService,serverService,categoryService,newsService);
            await menu.Show();


        }
    }
}
