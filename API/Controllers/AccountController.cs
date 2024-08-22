using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOS;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context,ItokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async  Task<ActionResult<UserDto>>  Register(RegisterDto dto)
    {
        if(await UserExists(dto.username))
            return BadRequest("User is taken");

        using var hmac =new HMACSHA512();
        var appUser= new AppUser
        {
            UserName = dto.username.ToLower(),
            PasswordHashed= hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password)),
            PasswordSalt=hmac.Key 

        };
         context.Users.Add(appUser);
        await context.SaveChangesAsync();
        return new UserDto
        {
            Username = appUser.UserName,
            KnownAs = "",
            Gender = "",
            Token = tokenService.CreateToken(appUser)

        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        var user = await context.Users.FirstAsync(u=>u.UserName== dto.Username);
        if (user==null)
            return BadRequest("no username found");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var coputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        for (int i = 0; i < coputedHash.Length; i++)
        {
            if(coputedHash[i]!= user.PasswordHashed[i])
            return Unauthorized("wrong password");
        }
        return new UserDto{
            Username=user.UserName,
            KnownAs="",
            Gender="",
            Token=tokenService.CreateToken(user)
        
            };
    }

    private async Task<bool> UserExists(string UserName)
    {
       return await  context.Users.AnyAsync(u=>u.UserName.ToLower()==UserName.ToLower());
    }
}