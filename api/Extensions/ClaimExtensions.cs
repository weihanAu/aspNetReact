using System.Security.Claims;

namespace api.Extensions
{
  public static class ClaimExtensions
  {
    public static string GetUserName(this ClaimsPrincipal user)
    {
      if (user == null) return string.Empty;
      return user.FindFirst(ClaimTypes.GivenName)?.Value ?? String.Empty;
    }
  }
}