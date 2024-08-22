
using API.Entities;

namespace API;
public interface ItokenService
{
    string CreateToken(AppUser user);
    
}