using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Helper.Interface
{
    public interface IArticleActionHelper
    {
        Task HandleSaveArticleAsync();
        Task HandleLikeArticleAsync();
        Task HandleDislikeArticleAsync();
        Task HandleReportArticleAsync();
    }
}
