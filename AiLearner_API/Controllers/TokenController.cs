using AiLearner_API.Services;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AiLearner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IUnitOfWork unitOfWork, JwtTokenService jwtTokenService) : ControllerBase
    {
        private readonly JwtTokenService _jwtService = jwtTokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshTokenString)
        {
            //get refresh token from db and verify it
            string decodedToken = WebUtility.UrlDecode(refreshTokenString);
            RefreshToken? refreshToken = await _unitOfWork.RefreshToken.VarifyRefreshToken(decodedToken);
            if (refreshToken is null) 
                return BadRequest("Invalid refresh token");

            User? user = await _unitOfWork.Users.GetByIdAsync(refreshToken.UserId);

            //generate new token and refresh token
            var newToken = _jwtService.GenerateJwtToken(user);
            var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id!);

            //add new token and refresh token to HttpOnly cookies


            //update old refresh token to include new token id and revoke it
            refreshToken.ReplacedByTokenId = newRefreshToken.Id;
            refreshToken.Revoked = true;
            _unitOfWork.RefreshToken.Update(refreshToken);
            int changes = await _unitOfWork.CompleteAsync();

            if(changes <= 0) 
                return BadRequest("Token Refresh Failed");

            
            _jwtService.AppendCookie(Response, newToken, newRefreshToken);
            
            return Ok("Token Refreshed");
        }
    }
}
