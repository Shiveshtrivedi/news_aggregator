using news_application.Enum;
using System.ComponentModel.DataAnnotations;

namespace news_application.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_])(?=.*[a-z])(?=.*[0-9]).*$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsTokenActive { get; set; } = false;
        public DateTime? TokenExpirationTime { get; set; }
        public DateTime? LastLogin { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
