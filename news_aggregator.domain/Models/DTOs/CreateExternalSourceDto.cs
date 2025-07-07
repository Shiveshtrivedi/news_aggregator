using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models.DTOs
{
    public class CreateExternalSourceDto
    {
        public string ExternalSourceName { get; set; }
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public bool IsActive { get; set; }
    }

}
