using Azure;
using DataAccessLayer.dbContext;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AiLearner_API.Services
{
    public class JwtTokenService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id!)
        }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration set to 1 hour
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = GenerateRefreshTokenString(),
                Expiration = DateTime.UtcNow.AddDays(1),
                Created = DateTime.UtcNow,
                Revoked = false
            };

            await _unitOfWork.RefreshToken.CreateRefreshToken(refreshToken);

            return refreshToken;
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public void AppendCookie(HttpResponse response, string token, RefreshToken refreshToken)
        {
            response.Cookies.Append("AccessToken", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1),
                Secure = true,
                SameSite = SameSiteMode.None,

            });

            response.Cookies.Append("refreshToken", refreshToken.Token!, new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expiration,
                Secure = true,
                SameSite = SameSiteMode.None,
            });
        }   

    }
}
