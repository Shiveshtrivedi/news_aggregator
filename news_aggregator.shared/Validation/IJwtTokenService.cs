using Microsoft.Extensions.Configuration;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.Validation
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);
        ClaimsPrincipal? ValidateJwtToken(string token);


    }
}
