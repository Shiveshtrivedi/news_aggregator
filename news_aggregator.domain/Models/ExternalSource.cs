namespace news_application.Models
{
    public class ExternalSource
    {
        public int ExternalSourceId { get; set; }
        public string ExternalSourceName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime LastAccessed { get; set; }

        public string AuthParamName { get; set; } = "";     
        public string AuthLocation { get; set; } = "query";    

        public ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}

