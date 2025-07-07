using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.infrastructure.Adapter.ExternalNews.Interface;

namespace news_aggregator.infrastructure.Adapter.Builders
{
    public class NewsRequestBuilder : INewsRequestBuilder
    {
        public HttpRequestMessage BuildRequest(ExternalSourceDto source, string category, string keyword)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(category))
                queryParams.Add($"category={Uri.EscapeDataString(category)}");

            if (!string.IsNullOrWhiteSpace(keyword))
                queryParams.Add($"q={Uri.EscapeDataString(keyword)}");

            if (source.AuthLocation == "query" && !string.IsNullOrWhiteSpace(source.AuthParamName))
                queryParams.Add($"{source.AuthParamName}={Uri.EscapeDataString(source.ApiKey)}");

            var separator = source.BaseUrl.Contains('?') ? "&" : "?";
            var fullUrl = $"{source.BaseUrl}{(queryParams.Any() ? separator + string.Join("&", queryParams) : "")}";

            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

            if (source.AuthLocation == "header" && !string.IsNullOrWhiteSpace(source.AuthParamName))
                request.Headers.Add(source.AuthParamName, source.ApiKey);

            return request;
        }
    }

}
