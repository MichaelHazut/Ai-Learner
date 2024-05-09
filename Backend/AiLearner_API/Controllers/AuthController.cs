using AiLearner_API.Services;
using DataAccessLayer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Security.Claims;

namespace AiLearner_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(JwtTokenService jwtTokenService) : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService = jwtTokenService;

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            // Read the tokens from the cookies
            var expiredToken = Request.Cookies["AccessToken"];
            var refreshTokenString = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(expiredToken) || string.IsNullOrEmpty(refreshTokenString))
            {
                // Extract the token from the Authorization header
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                {
                    expiredToken = authorizationHeader.ToString().Split(' ').Last();
                }
                // Extract the refresh token from the X-Refresh-Token header
                if (HttpContext.Request.Headers.TryGetValue("X-Refresh-Token", out var refreshToken))
                {
                    refreshTokenString = refreshToken;
                }
                else
                {
                    return BadRequest("Token is required.");
                }
            }
            if (string.IsNullOrEmpty(expiredToken) || string.IsNullOrEmpty(refreshTokenString))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                string newToken = await _jwtTokenService.RefreshJwtToken(expiredToken, refreshTokenString);

                // Create a new refresh token
                var principal = _jwtTokenService.ValidateToken(expiredToken, false);
                var userId = (principal.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    ?? throw new SecurityTokenException("User ID is missing in the token.");

                RefreshToken newRefreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(userId);
                await _jwtTokenService.RevokeRefreshToken(refreshTokenString, newRefreshToken.Token!);
                // Append new tokens to the response as cookies
                _jwtTokenService.AppendCookie(Response, newToken, newRefreshToken);

                // Return only a status code if you're setting HTTP-only cookies
                return Ok(new { NewToken = newToken });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            try
            {
                // Extract the token from the HTTP-only cookie
                var token = Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                {
                    // Extract the token from the Authorization header
                    if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                    {
                        token = authorizationHeader.ToString().Split(' ').Last();
                    }
                    else
                    {
                        return Unauthorized(new { message = "No token provided." });
                    }
                }
                var token2 = "";
                if (HttpContext.Request.Headers.TryGetValue("X-Refresh-Token", out var refreshToken))
                {
                    token2 = refreshToken;
                }

                // Validate the token
                var principal = _jwtTokenService.ValidateToken(token);
                if (principal == null)
                {
                    // The token is invalid or expired
                    return Unauthorized(new { IsAuthenticated = false }); ;
                }
                else
                {
                    var userId = (principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    // The token is valid
                    return Ok(new { IsAuthenticated = true, UserId = userId });
                }
            }
            catch (SecurityTokenException)
            {
                // The token is invalid or expired
                return Unauthorized(new { IsAuthenticated = false, UserId = "" });
            }
            catch (Exception ex)
            {
                // An unexpected error occurred
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            // Extract the refresh token from the HTTP-only cookie
            var refreshTokenString = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshTokenString))
            {
                return BadRequest("Refresh token is required.");
            }

            try
            {
                // Revoke the refresh token
                await _jwtTokenService.RevokeRefreshToken(refreshTokenString);

                // Remove the cookies from the response
                var cookieOptions = new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                };

                Response.Cookies.Delete("AccessToken", cookieOptions);
                Response.Cookies.Delete("refreshToken", cookieOptions);

                return Ok();
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

    }

}
