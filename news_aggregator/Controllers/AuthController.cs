using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_aggregator.application;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException.UserException;
using news_aggregator.shared.Validation;
using static news_aggregator.application.AuthService;

namespace news_aggregator.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] UserDTO userDTO)
        {
            try
            {
                var errors = SignUpDtoValidator.Validate(userDTO);

                if (errors.Any())
                {
                    return BadRequest(new { Errors = errors });
                }
                var result = await _authService.SignupAsync(userDTO);

                return Ok(result);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch(InvalidUserRoleException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var errors = LoginDtoValidator.Validate(loginDto);

                if (errors.Any())
                {
                    return BadRequest(new { Errors = errors });
                }
                var userDto = await _authService.LoginAsync(loginDto);
                return Ok(userDto);
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, "Please provide email and passowrd");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new { message = "Token is required" });

                if (token.StartsWith("Bearer "))
                    token = token.Substring("Bearer ".Length);

                await _authService.LogoutAsync(token);
                return Ok(new { message = "Logout successful" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
