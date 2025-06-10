using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models.DTOs
{
    public class LoginResponse
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? SessionToken { get; set; }
        public DateTime? TokenExpirationTime { get; set; }
    }
}
