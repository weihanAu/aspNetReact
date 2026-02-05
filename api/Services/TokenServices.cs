using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Interfaces;
using api.models;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class TokenServices : ITokenService
{
  private readonly IConfiguration _config;
  private readonly SymmetricSecurityKey _key;
  public TokenServices(IConfiguration config)
  {
    _config = config;
    _key = new SymmetricSecurityKey(
      System.Text.Encoding.UTF8.GetBytes(_config["JWT:SiginKey"])
    );
  }
  public string CreateToken(AppUser user)
  {
    var claims = new List<Claim>{
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
    };
    var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.AddDays(7),
      SigningCredentials = creds,
      Issuer = _config["JWT:Issuer"],
      Audience = _config["JWT:Audience"]
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}