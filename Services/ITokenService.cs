using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace LoginManager.Services;

public interface ITokenService
{
    string GenerateToken(User user, IList<string>? roles);
}

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user, IList<string>? roles)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        if(roles is {Count: > 0})
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!));
        
        if (key is null)
            throw new ArgumentNullException(nameof(key), "Key cannot be null");

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}