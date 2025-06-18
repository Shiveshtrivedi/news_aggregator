using news_aggregator.domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.Validation
{
    public class SignUpDtoValidator
    {
        public static List<string> Validate(UserDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.UserName))
                errors.Add("Name is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("Email is required.");
            else if (!dto.Email.Contains('@') || !dto.Email.Contains('.'))
                errors.Add("Email format is invalid.");

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                errors.Add("Password is required.");
            }
            else
            {
                if (dto.Password.Length < 8)
                    errors.Add("Password must be at least 8 characters long.");

                if (!dto.Password.Any(char.IsUpper))
                    errors.Add("Password must contain at least one uppercase letter.");

                if (!dto.Password.Any(char.IsLower))
                    errors.Add("Password must contain at least one lowercase letter.");

                if (!dto.Password.Any(char.IsDigit))
                    errors.Add("Password must contain at least one number.");

                if (!dto.Password.Any(c => !char.IsLetterOrDigit(c)))
                    errors.Add("Password must contain at least one special character.");
            }

            return errors;
        }

    }
}
