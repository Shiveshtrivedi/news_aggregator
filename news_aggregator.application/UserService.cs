using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User?> GetUserByIdAsync(int userId)
        {
            var user = _userRepository.GetByIdAsync(userId);
            return user;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = _userRepository.GetAllAsync();
            return users;
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }
        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteAsync(userId);
        }
    }
}
