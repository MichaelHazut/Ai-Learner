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
    public class UserController(IUnitOfWork unitOfWork, JwtTokenService jwtTokenService, CachingService cachingService) : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly JwtTokenService _jwtTokenService = jwtTokenService;
        private readonly CachingService _cachingService = cachingService;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //check if user already exists
            User user = await _unitOfWork.Users.NewUser(userData.Email, userData.Password);
            
            //change is 0 if user already exists
            int changes = await _unitOfWork.CompleteAsync();
            if (changes <= 0)
                return BadRequest("User Registration Failed");

            //cache user in memory
            _cachingService.CacheItem(user.Email!, user);

            return Ok("User Registered Successfully");
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDto userData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //check if user is already cached in memory 
            bool isCached = _cachingService.TryGetCachedItem(userData.Email, out User? user);
            if (isCached is true)            
                return Ok(user);
            
            //get user from db and verify credentials
            user = await _unitOfWork.Users.LogIn(userData.Email, userData.Password);
            if (user is null) 
                return Unauthorized("Invalid credentials");

            
            //generate jwtToken and refreshToken
            var jwtToken = _jwtTokenService.GenerateJwtToken(user);
            var refreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(user.Id!);

            //cache user in memory
            _cachingService.CacheItem(user.Email!, user);

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
