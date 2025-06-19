using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.ExternalApi
{
    public class TheNewsApiResponse
    {
        [JsonPropertyName("meta")]
        public MetaData Meta { get; set; }
        [JsonPropertyName("data")]
        public List<TheNewsArticle> Data { get; set; }
    }

    public class MetaData
    {
        public int Found { get; set; }
        public int Returned { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
    }

    public class TheNewsArticle
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("snippet")]
        public string Snippet { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("image_url")]
        public string Image_Url { get; set; }

        [JsonPropertyName("published_at")]
        public DateTime Published_At { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }
    }

}
