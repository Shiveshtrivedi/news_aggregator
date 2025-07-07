using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console
{
    public class AppServices
    {
        public IAuthService AuthService { get; set; }
        public IServerService ServerService { get; set; }
        public ICategoryService CategoryService { get; set; }
        public INewsService NewsService { get; set; }
        public ISavedArticleService SavedArticleService { get; set; }
        public ISearchArticleService SearchArticleService { get; set; }
        public INotificationService NotificationService { get; set; }
        public IBlockedKeywordService BlockedKeywordService { get; set; }
        public IUserKeywordService UserKeywordService { get; set; }
    }
}
