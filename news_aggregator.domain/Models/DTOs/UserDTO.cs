using news_application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        //[ComplexityAttribute(ValidateUserName = true)]
        public string UserName { get; set; } = string.Empty;
        //[ComplexityAttribute(ValidateEmail = true)]
        public string Email { get; set; } = string.Empty;
        //[ComplexityAttribute(ValidatePassword = true)]
        public string? Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

    }
}
