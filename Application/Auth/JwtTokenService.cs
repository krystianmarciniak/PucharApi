using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PucharApi.Domain;

namespace PucharApi.Application.Auth;

public class JwtTokenService
{
  private readonly IConfiguration _config;

  public JwtTokenService(IConfiguration config) => _config = config;

  public string CreateToken(User user)
  {
    var jwt = _config.GetSection("Jwt");
    var key = jwt["Key"]!;
    var issuer = jwt["Issuer"]!;
    var audience = jwt["Audience"]!;
    var expiresMinutes = int.Parse(jwt["ExpiresMinutes"] ?? "120");


    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
