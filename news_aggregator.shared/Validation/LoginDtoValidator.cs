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
            if (string.IsNullOrWhiteSpace(dto.Password))
                errors.Add("Password is required.");
            
            return errors;

        }
    }
}
