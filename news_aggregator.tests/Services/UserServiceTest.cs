using AutoMapper;
using Moq;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application;
using news_aggregator.tests.Helpers;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.domain.Models.DTOs;

namespace news_aggregator.tests.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUserDto_WhenUserExists()
        {
            var user = UserMockData.GetUser();
            var expectedDto = UserMockData.GetUserDto();

            _userRepositoryMock.Setup(r => r.GetByIdAsync(user.UserId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(expectedDto);

            var result = await _userService.GetUserByIdAsync(user.UserId);

            Assert.NotNull(result);
            Assert.Equal(expectedDto.UserId, result.UserId);
            Assert.Equal(expectedDto.Role, result.Role);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null!);

            var result = await _userService.GetUserByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnMappedUserDtos()
        {
            var users = UserMockData.GetUsers();
            var expectedDtos = UserMockData.GetUserDtos();

            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDTO>>(users)).Returns(expectedDtos);

            var result = await _userService.GetAllUsersAsync();

            Assert.Equal(expectedDtos.Count, result.Count());
        }

        [Fact]
        public async Task AddUserAsync_ShouldInvokeRepository()
        {
            var newUser = UserMockData.GetUser();

            _userRepositoryMock.Setup(r => r.AddAsync(newUser)).Returns(Task.CompletedTask).Verifiable();

            await _userService.AddUserAsync(newUser);

            _userRepositoryMock.Verify();
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldInvokeRepository()
        {
            var user = UserMockData.GetUser();

            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask).Verifiable();

            await _userService.UpdateUserAsync(user);

            _userRepositoryMock.Verify();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldInvokeRepository()
        {
            _userRepositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.FromResult(true)).Verifiable();

            await _userService.DeleteUserAsync(1);

            _userRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
