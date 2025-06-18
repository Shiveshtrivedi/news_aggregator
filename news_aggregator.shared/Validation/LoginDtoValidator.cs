using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.domain.Models.DTOs;

namespace news_aggregator.shared.Validation
{
    public class LoginDtoValidator 
    {
        public static List<string> Validate(LoginDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("Email is required.");
            else if (!dto.Email.Contains('@'))
                errors.Add("Email is invalid.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                errors.Add("Password is required.");
            else if (dto.Password.Length < 6)
                errors.Add("Password must be at least 6 characters long.");

            return errors;

        }
    }
}
