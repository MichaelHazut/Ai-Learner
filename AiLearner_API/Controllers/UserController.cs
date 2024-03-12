using AiLearner_API.Services;
using DataAccessLayer.DTO;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Common;
using System.Reflection.Metadata.Ecma335;

namespace AiLearner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUnitOfWork unitOfWork, JwtTokenService jwtTokenService) : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly JwtTokenService _jwtTokenService = jwtTokenService;


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _unitOfWork.Users.NewUser(userData.Email, userData.Password);
            int changes = await _unitOfWork.CompleteAsync();

            return changes > 0 ? Ok("User Registered Successfully") : BadRequest("User Registration Failed");
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDto userData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //get user from db and verify credentials
            User? user = await _unitOfWork.Users.LogIn(userData.Email, userData.Password);
            if (user is null) return Unauthorized("Invalid credentials");

            //generate jwtToken and refreshToken
            var jwtToken = _jwtTokenService.GenerateJwtToken(user);
            var refreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(user.Id!);

            await _unitOfWork.CompleteAsync();

            //add jwtToken and refreshToken to HttpOnly cookies
            _jwtTokenService.AppendCookie(Response, jwtToken, refreshToken);
            

            return Ok();

        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isDeleted = await _unitOfWork.DeleteUserAsync(userId);

            return (isDeleted is true) ? Ok("User Deleted Successfully") : NotFound("User Not Found");
        }
    }

}
