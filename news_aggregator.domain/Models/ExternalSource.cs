namespace news_application.Models
{
    public class ExternalSource
    {
        public int ExternalSourceId { get; set; }
        public string ExternalSourceName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}
