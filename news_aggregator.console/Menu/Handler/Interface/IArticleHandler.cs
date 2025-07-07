using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler.Interface
{
    public interface IArticleHandler
    {
        Task ShowAndHandleArticlesAsync(string category, DateTime startDate, DateTime endDate);

    }
}
