using LibraryManagement.Application.Auth.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GenerateTokenAsync(
            string userId,
            string email,
            string fullName,
            IList<string> roles)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var secretKey = _configuration["Jwt:SecretKey"];
            var expirationMinutesValue = _configuration["Jwt:ExpirationMinutes"];

            if (string.IsNullOrWhiteSpace(issuer))
                throw new InvalidOperationException("JWT issuer is missing.");

            if (string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("JWT audience is missing.");

            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JWT secret key is missing.");

            if (!int.TryParse(expirationMinutesValue, out var expirationMinutes))
                expirationMinutes = 120;

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, fullName),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            var tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Task.FromResult(tokenValue);
        }
    }
}
