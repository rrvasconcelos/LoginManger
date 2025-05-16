using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginManager.Configuration.Options;
using LoginManager.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LoginManager.Services;

public interface ITokenService
{
    (string Token, DateTime Expires) GenerateToken(User user, IList<string>? roles);
}

public class TokenService(IConfiguration configuration, IOptions<JwtOptions> options, ILogger<TokenService> logger) : ITokenService
{
    public (string Token, DateTime Expires) GenerateToken(User user, IList<string>? roles)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        if(roles is {Count: > 0})
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        
        if (key is null)
            throw new ArgumentNullException(nameof(key), "Key cannot be null");

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var expires = DateTime.Now.AddMinutes(options.Value.ExpirationInMinutes);
        
        var token = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}