using news_application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models.DTOs
{
    public class LoginDTO
    {

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? Token { get; set; }
    }
}
