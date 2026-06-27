using FluentValidation;
using LibraryManagement.Application.Auth.DTOs;
using LibraryManagement.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginRequest> _loginRequestValidator;

        public AuthController(
            IAuthService authService,
            IValidator<LoginRequest> loginRequestValidator)
        {
            _authService = authService;
            _loginRequestValidator = loginRequestValidator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var validationResult = await _loginRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new
                    {
                        field = error.PropertyName,
                        message = error.ErrorMessage
                    });

                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors
                });
            }

            var response = await _authService.LoginAsync(request);

            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<CurrentUserResponse> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var fullName = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

            var roles = User.FindAll(ClaimTypes.Role)
                .Select(claim => claim.Value)
                .ToList();

            return Ok(new CurrentUserResponse
            {
                UserId = userId,
                Email = email,
                FullName = fullName,
                Roles = roles
            });
        }
    }
}
