
using API.Entities;

namespace API;
public interface ItokenService
{
    Task<string> CreateToken(AppUser user);
    
}