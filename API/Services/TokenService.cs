using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API;

public class TokenService(IConfiguration config, UserManager<AppUser> userManager) : ItokenService
{
    public async Task<string> CreateToken(AppUser user)
    {
        var tokenKey = config["tokenKey"] ?? throw new Exception("Cannot access token key");
        if(tokenKey.Length <64 )throw new Exception ("token key too short");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier,user.Id.ToString()),
            new (ClaimTypes.Name,user.UserName)
        };
        
        var roles = await userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
        
            
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires= DateTime.UtcNow.AddDays(7),
            SigningCredentials=creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token=tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}