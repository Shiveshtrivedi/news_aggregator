using Microsoft.IdentityModel.Tokens;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_application.Enum;
using news_application.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using BCr = BCrypt.Net;
using news_aggregator.shared.Authentication;
using news_aggregator.shared.CustomException;
using news_aggregator.shared.Validation.Interface;
using news_aggregator.shared.CustomException.UserException;
using AutoMapper;


namespace news_aggregator.application
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;


        public AuthService(IUserRepository userRepository, IJwtTokenService jwtService, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserDTO> SignupAsync(UserDTO userDTO)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userDTO.Email);
            if (existingUser != null)
                throw new UserAlreadyExistsException();

            if (userDTO.Role != UserRole.Admin && userDTO.Role != UserRole.User)
                throw new InvalidUserRoleException();


            var passwordHash = _passwordHasher.HashPassword(userDTO.Password);

            var newUser = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Password = passwordHash,
                Role = userDTO.Role
            };

            await _userRepository.AddAsync(newUser);

            return _mapper.Map<UserDTO>(newUser);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                throw new ArgumentException("Email and password must be provided.");

            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.Password))
                throw new InvalidCredentialsException();

            var token = _jwtService.GenerateJwtToken(user);

            var loginResponseDto = _mapper.Map<LoginResponseDto>(user);
            loginResponseDto.Token = token;

            return loginResponseDto;
        }

        public async Task LogoutAsync(string token)
        {
            var principal = _jwtService.ValidateJwtToken(token);
            var userId = principal?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userId == null)
                throw new UserNotFoundException();

            var user = await _userRepository.GetByIdAsync(int.Parse(userId));
            user.IsTokenActive = false;
            await _userRepository.UpdateAsync(user);
        }

     
    }
}
