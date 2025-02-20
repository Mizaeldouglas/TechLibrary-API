using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Infrastructure.Security.Tokens.Access;

public class JwtTokenGenerator
{
    public string GenerateToken(User user)
    {

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(1),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(CreateSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }

    private static SymmetricSecurityKey CreateSecurityKey()
    {
        var key = "sVmT60viFR1YH5xnlWYA3i9Up7K0PPub"u8.ToArray();
        return new SymmetricSecurityKey(key);
    }
}