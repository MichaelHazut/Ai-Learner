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
            new Claim(ClaimTypes.NameIdentifier, user.Id!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
            await _unitOfWork.CompleteAsync();
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
                Expires = DateTime.UtcNow.AddDays(1),
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
        public ClaimsPrincipal ValidateToken(string token, bool validateLifetime = true)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.Zero
            };
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }

        // Checks if the refresh token is valid, and if so, creates a new JWT token
        public async Task<string> RefreshJwtToken(string expiredToken, string refreshTokenString)
        {
            var userClaim = ValidateToken(expiredToken, false)?.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var refreshToken = await _unitOfWork.RefreshToken.VarifyRefreshToken(refreshTokenString);
            if (refreshToken == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(refreshToken.UserId);
            if (user == null)
            {
                throw new SecurityTokenException("User does not exist");
            }

            // Optionally, you could also verify if the refresh token matches the one stored in the database

            return GenerateJwtToken(user); // Reuse the method to generate a new JWT token
        }

        public async Task<bool> RevokeRefreshToken(string oldRefreshTokenString, string newRefreshToken = "")
        {
            ArgumentNullException.ThrowIfNull(oldRefreshTokenString);

            var oRefreshToken = await _unitOfWork.RefreshToken.VarifyRefreshToken(oldRefreshTokenString);
            var nRefreshToken = await _unitOfWork.RefreshToken.VarifyRefreshToken(newRefreshToken);

            if (oRefreshToken == null) return false;

            if (oRefreshToken != null && nRefreshToken != null)
            {
                oRefreshToken!.ReplacedByTokenId = nRefreshToken!.Id;
            }

            oRefreshToken!.Revoked = true;
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }
}
