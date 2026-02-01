using api.models;
using Api.Dtos.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{ 
    [ApiController]
    [Route("api/account")]
    public class AccountController:ControllerBase
    {
      private readonly UserManager<AppUser> _userManager;
      public AccountController(UserManager<AppUser> userManager)
      {
        _userManager = userManager;
      }
      
      [HttpPost("register")]
      public async Task<IActionResult> Register([FromBody]CreateUserDto registerDto)
      {
        var user = new AppUser
        {
          UserName = registerDto.Username,
          Email = registerDto.Email,
          PasswordHash = registerDto.Password
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
          return BadRequest(result.Errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
        {
          return BadRequest(roleResult.Errors);
        }

        return Ok(roleResult);
      }
    }
}