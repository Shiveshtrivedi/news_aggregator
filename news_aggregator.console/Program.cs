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

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IHttpClientFactoryWrapper clientFactory = new HttpClientFactory(configuration);

            var services = new AppServices
            {
                AuthService = new AuthService(clientFactory),
                ServerService = new ServerService(clientFactory),
                CategoryService = new CategoryService(clientFactory),
                NewsService = new NewsService(clientFactory),
                SavedArticleService = new SavedArticleService(clientFactory),
                SearchArticleService = new SearchArticleService(clientFactory),
                NotificationService = new NotificationService(clientFactory),
                BlockedKeywordService = new BlockedKeywordService(clientFactory),
                UserKeywordService = new UserKeywordService(clientFactory)
            };


            IMenu menu = new MainMenu(services);
            await menu.Show();


        }
    }
}
