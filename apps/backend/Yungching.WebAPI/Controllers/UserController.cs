using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yungching.Application.DTOs.Users;
using Yungching.Application.Auth;
using Yungching.Domain.Models;
using Yungching.Domain.ValueObjects;

namespace Yungching.WebAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: user/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _userService.RegisterUserAsync(dto);
            if (result == null) 
            {
                return Conflict("Email already exists.");
            }

            return Ok(result);
        }

        // POST: user/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _userService.LoginUserAsync(dto);
            if (result == null) 
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(result);
        }

        // PUT: user/update
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            var result = await _userService.UpdateUserAsync(user);
            if (result == null) 
            {
                return NotFound("User not found.");
            }

            return Ok(result);
        }

        // DELETE: user/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) 
            {
                return NotFound("User not found or delete failed.");
            }

            return Ok("User deleted.");
        }

        // GET: user/email-exists?email=user@example.com
        [HttpGet("email-exists")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            try
            {
                var emailRequest = new Email(email);
                var isExisted = await _userService.IsEmailExistedAsync(emailRequest);
                return Ok(new { isExisted });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
