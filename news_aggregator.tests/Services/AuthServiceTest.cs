using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.shared.Validation.Interface;
using news_aggregator.shared.Authentication;
using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using news_application.Enum;
using news_aggregator.shared.CustomException.UserException;
using news_aggregator.shared.CustomException;
using news_aggregator.tests.Helpers;
using news_aggregator.application.Auth;

namespace news_aggregator.tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IJwtTokenService> _jwtMock = new();
        private readonly Mock<IPasswordHasher> _hasherMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService(
                _userRepoMock.Object,
                _jwtMock.Object,
                _hasherMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task SignupAsync_WithNewUser_ReturnsUserDTO()
        {
            var newUserDto = UserMockDataGenerator.GetTestUserDto();

            _userRepoMock.Setup(r => r.GetByEmailAsync(newUserDto.Email))
                .ReturnsAsync((User)null!);

            _hasherMock.Setup(h => h.HashPassword(It.IsAny<string>()))
                .Returns("hashedPassword");

            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns(newUserDto);

            var result = await _authService.SignupAsync(newUserDto);

            Assert.Equal(newUserDto.Email, result.Email);
        }

        [Fact]
        public async Task SignupAsync_WhenUserExists_ThrowsUserAlreadyExistsException()
        {
            var existingUser = new User { Email = "testuser@example.com" };
            _userRepoMock.Setup(r => r.GetByEmailAsync(existingUser.Email))
                .ReturnsAsync(existingUser);

            var dto = UserMockDataGenerator.GetTestUserDto();

            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _authService.SignupAsync(dto));
        }

        [Fact]
        public async Task SignupAsync_WithInvalidRole_ThrowsInvalidUserRoleException()
        {
            var dto = UserMockDataGenerator.GetTestUserDto();
            dto.Role = (UserRole)99;

            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User)null!);

            await Assert.ThrowsAsync<InvalidUserRoleException>(() => _authService.SignupAsync(dto));
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponseDto()
        {
            var loginDto = UserMockDataGenerator.GetValidLoginDto();
            var user = new User { Email = loginDto.Email, Password = "hashed" };

            _userRepoMock.Setup(r => r.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            _hasherMock.Setup(h => h.VerifyPassword(loginDto.Password, user.Password))
                .Returns(true);

            _jwtMock.Setup(j => j.GenerateJwtToken(user))
                .Returns("mocked-token");

            var expectedResponse = UserMockDataGenerator.GetLoginResponseDto();
            expectedResponse.Token = "mocked-token";

            _mapperMock.Setup(m => m.Map<LoginResponseDto>(user))
                .Returns(expectedResponse);

            var result = await _authService.LoginAsync(loginDto);

            Assert.Equal("mocked-token", result.Token);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidCredentials_ThrowsInvalidCredentialsException()
        {
            var loginDto = UserMockDataGenerator.GetValidLoginDto();
            var user = new User { Email = loginDto.Email, Password = "wrong" };

            _userRepoMock.Setup(r => r.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            _hasherMock.Setup(h => h.VerifyPassword(loginDto.Password, user.Password))
                .Returns(false);

            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.LoginAsync(loginDto));
        }

        [Fact]
        public async Task LogoutAsync_WithValidToken_DisablesUserToken()
        {
            var claims = new List<Claim> { new Claim("UserId", "1") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var user = new User { UserId = 1, IsTokenActive = true };

            _jwtMock.Setup(j => j.ValidateJwtToken("token"))
                .Returns(principal);

            _userRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(user);

            _userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            await _authService.LogoutAsync("token");

            Assert.False(user.IsTokenActive);
        }

        [Fact]
        public async Task LogoutAsync_WithInvalidToken_ThrowsUserNotFoundException()
        {
            _jwtMock.Setup(j => j.ValidateJwtToken(It.IsAny<string>()))
                .Returns((ClaimsPrincipal)null!);

            await Assert.ThrowsAsync<UserNotFoundException>(() => _authService.LogoutAsync("invalid"));
        }
    }
}
