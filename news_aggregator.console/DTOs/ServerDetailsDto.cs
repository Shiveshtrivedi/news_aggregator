using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Models
{
    public class ServerDetailsDto
    {
        public string ExternalSourceName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}
