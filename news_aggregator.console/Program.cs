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

            IAuthService authService = new AuthService(clientFactory);
            IServerService serverService = new ServerService(clientFactory);
            ICategoryService categoryService= new CategoryService(clientFactory);
            INewsService newsService = new NewsService(clientFactory);
            ISavedArticleService savedArticleService = new SavedArticleService(clientFactory);
            ISearchArticleService searchArticleService = new SearchArticleService(clientFactory);
            INotificationService notificationService= new NotificationService(clientFactory);

            IMenu menu = new MainMenu(authService,serverService,categoryService,newsService,savedArticleService,searchArticleService,notificationService);
            await menu.Show();


        }
    }
}
