using news_aggregator.domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserDTO> SignupAsync(UserDTO userDTO);
        Task<UserDTO> LoginAsync(LoginDTO loginDto);
        Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string token, string refreshToken);
    }
}
