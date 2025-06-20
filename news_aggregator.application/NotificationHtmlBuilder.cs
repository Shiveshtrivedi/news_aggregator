using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class NotificationHtmlBuilder : INotificationHtmlBuilder
    {
        public string Build(string category, IEnumerable<NewsArticle> articles)
        {
            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine($"<h3>Latest {category} News</h3>");

            foreach (var article in articles)
            {
                stringbuilder.AppendLine("<div style=\"margin-bottom: 15px;\">");
                stringbuilder.AppendLine($"<strong>Title:</strong> {article.Title}<br/>");
                stringbuilder   .AppendLine($"<strong>URL:</strong> <a href=\"{article.Url}\" target=\"_blank\">{article.Url}</a><br/>");
                stringbuilder.AppendLine("</div>");
            }

            return stringbuilder.ToString();
        }
    }
}
