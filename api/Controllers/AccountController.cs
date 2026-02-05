using api.Interfaces;
using api.models;
using Api.Dtos.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{ 
    [ApiController]
    [Route("api/account")]
    public class AccountController:ControllerBase
    {
      private readonly UserManager<AppUser> _userManager;
      private readonly ITokenService _tokenService;
      private readonly SignInManager<AppUser> _signInManager;
      public AccountController(UserManager<AppUser> userManager,ITokenService tokenService, SignInManager<AppUser> signInManager)
      {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
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

        return Ok(new NewUserDto{
          Username = user.UserName,
          Email = user.Email,
          Token = _tokenService.CreateToken(user)
        });
      }

      [HttpPost("login")]
      public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
    {
      if(!ModelState.IsValid) return BadRequest(ModelState);
      var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName == loginDto.Username);
      if(user == null) return Unauthorized("Invalid username or password");
      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
      if(!result.Succeeded) return Unauthorized("Invalid username or password");
      return Ok(new NewUserDto{
        Username = user.UserName,
        Email = user.Email,
        Token = _tokenService.CreateToken(user)
      });
    }
    }
}