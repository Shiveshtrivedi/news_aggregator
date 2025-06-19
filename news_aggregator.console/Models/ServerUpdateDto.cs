using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Models
{
    public class ServerUpdateDto
    {
        public int Id { get; set; }
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public string AuthParamName { get; set; }
        public string AuthLocation { get; set; } = "query";
        public bool IsActive { get; set; }

    }
}
