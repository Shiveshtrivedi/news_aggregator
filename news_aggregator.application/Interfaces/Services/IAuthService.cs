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
        Task<LoginResponseDto> LoginAsync(LoginDTO loginDto);
        Task LogoutAsync(string token);
    }
}
