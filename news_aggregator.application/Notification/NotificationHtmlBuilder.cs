using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Notification
{
    public class NotificationHtmlBuilder : INotificationHtmlBuilder
    {
        public string Build(string category, IEnumerable<NewsArticleDto> articles)
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

        public string BuildReportNotification(NewsArticleDto? article, string reportMessage, string? reportedBy)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h3> Article Reported</h3>");
            sb.AppendLine($"<strong>Reported By:</strong> {reportedBy}<br/>");
            sb.AppendLine($"<strong>Title:</strong> {article.Title}<br/>");
            sb.AppendLine($"<strong>Category:</strong> {article.Category}<br/>");
            sb.AppendLine($"<strong>Published At:</strong> {article.PublishedAt}<br/>");
            sb.AppendLine($"<strong>Source:</strong> {article.Source}<br/>");
            sb.AppendLine($"<strong>URL:</strong> <a href=\"{article.Url}\" target=\"_blank\">{article.Url}</a><br/>");
            sb.AppendLine("<hr/>");
            sb.AppendLine($"<strong>Report Message:</strong><br/>{reportMessage}");

            return sb.ToString();
        }
    }
}
