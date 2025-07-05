using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public static class ExternalSourceMockDataGenerator
    {
        public static ExternalSourceDto GetExternalSourceDto(int id = 1, string name = "TheNewsAPI")
        {
            return new ExternalSourceDto
            {
                ExternalSourceId = id,
                ExternalSourceName = name,
                ApiKey = "sample-api-key",
                BaseUrl = "https://newsapi.org",
                IsActive = true,
                LastAccessed = DateTime.UtcNow,
                AuthParamName = "apiKey",
                AuthLocation = "query"
            };
        }

        public static CreateExternalSourceDto GetCreateExternalSourceDto(string name = "NewsAPI")
        {
            return new CreateExternalSourceDto
            {
                ExternalSourceName = name,
                ApiKey = "new-api-key",
                BaseUrl = "https://newsource.com",
                IsActive = true
            };
        }

        public static UpdateExternalSourceDto GetUpdateExternalSourceDto()
        {
            return new UpdateExternalSourceDto
            {
                ExternalSourceName = "UpdatedSource",
                ApiKey = "updated-key",
                BaseUrl = "https://updatedsource.com",
                IsActive = false,
                LastAccessed = DateTime.UtcNow.AddDays(-1),
                AuthParamName = "auth",
                AuthLocation = "header"
            };
        }

        public static ExternalSource GetExternalSourceEntity(int id = 1)
        {
            return new ExternalSource
            {
                ExternalSourceId = id,
                ExternalSourceName = "OriginalSource",
                ApiKey = "original-key",
                BaseUrl = "https://original.com",
                IsActive = true,
                LastAccessed = DateTime.UtcNow
            };
        }


    }
}
