using news_aggregator.domain.Models.DTOs;
using news_application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public class UserMockDataGenerator
    {
        public static UserDTO GetTestUserDto()
        {
            return new UserDTO
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "testuser@example.com",
                Password = "Password123",
                Role = UserRole.User,
                Token = "mocked-jwt-token"
            };
        }

        public static LoginDTO GetValidLoginDto()
        {
            return new LoginDTO
            {
                Email = "testuser@example.com",
                Password = "Password123"
            };
        }

        public static LoginDTO GetInvalidLoginDto()
        {
            return new LoginDTO
            {
                Email = "wrong@example.com",
                Password = "WrongPassword"
            };
        }

        public static LoginResponseDto GetLoginResponseDto()
        {
            return new LoginResponseDto
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "testuser@example.com",
                Role = UserRole.User,
                Token = "mocked-jwt-token"
            };
        }
    }
}
