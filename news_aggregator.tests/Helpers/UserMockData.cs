using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public static class UserMockData
    {
        public static User GetUser() =>
            new User
            {
                UserId = 1,
                UserName = "johndoe",
                Email = "john@example.com",
                Password = "secure123"
            };

        public static List<User> GetUsers() =>
            new()
            {
                GetUser(),
                new User
                {
                    UserId = 2,
                    UserName = "janesmith",
                    Email = "jane@example.com",
                    Password = "secure456"
                }
            };

        public static UserDTO GetUserDto() =>
            new UserDTO
            {
                UserId = 1,
                Email = "john@example.com",
                UserName = "johndoe"
            };

        public static List<UserDTO> GetUserDtos() =>
            new()
            {
                GetUserDto(),
                new UserDTO
                {
                    UserId = 2,
                    Email = "jane@example.com",
                    UserName = "janesmith"
                }
            };
    }
}
