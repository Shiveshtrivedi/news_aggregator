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
        public string Status { get; set; } = "";
        public string ApiKey { get; set; }
    }
}
