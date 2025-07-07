using news_aggregator.console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(string email, string password);
        Task<bool> SignUpAsync(UserDto userDto);
    }
}
