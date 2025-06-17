using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Http
{
    public class HttpClientFactory : IHttpClientFactoryWrapper
    {
        private readonly HttpClient client;
        public HttpClientFactory(IConfiguration configuration) 
        {
            var baseUrl = configuration["ApiSettings:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
                throw new Exception("BaseUrl is missing in configuration.");

            client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public HttpClient GetClient() => client;
    }
}
