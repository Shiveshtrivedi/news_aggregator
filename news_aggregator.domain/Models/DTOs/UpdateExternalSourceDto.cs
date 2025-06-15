using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models.DTOs
{
    public class UpdateExternalSourceDto
    {
        public string? ExternalSourceName { get; set; }
        public string? ApiKey { get; set; }
        public string? BaseUrl { get; set; }
        public string AuthParamName { get; set; } = "";
        public string AuthLocation { get; set; } = "query";
        public bool? IsActive { get; set; }
        public DateTime? LastAccessed { get; set; }
    }

}
