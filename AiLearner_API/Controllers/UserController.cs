using DataAccessLayer.DTO;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata.Ecma335;

namespace AiLearner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUnitOfWork unitOfWork) : ControllerBase
    {

        private readonly IUnitOfWork UnitOfWork = unitOfWork;


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await UnitOfWork.Users.NewUser(userData.Email, userData.Password);
            int changes = await UnitOfWork.CompleteAsync();

            return changes > 0 ? Ok("User Registered Successfully") : BadRequest("User Registration Failed");
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDto userData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isLogin = await UnitOfWork.Users.LogIn(userData.Email, userData.Password);

            return (isLogin is true) ? Ok("Login Successful") : Unauthorized("Email Or Password Are Invalid");
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isDeleted = await UnitOfWork.DeleteUserAsync(userId);

            return (isDeleted is true) ? Ok("User Deleted Successfully") : NotFound("User Not Found");
        }
    }

}
