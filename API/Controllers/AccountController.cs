using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOS;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context,ItokenService tokenService, IMapper mapper) : BaseApiController
{
    [HttpPost("register")] // account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
        using var hmac = new HMACSHA512();

        var user = mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.Username.ToLower();
        user.PasswordHashed=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordSalt=hmac.Key;
        //var result = await userManager.CreateAsync(user, registerDto.Password);
        await context.Users.AddAsync (user);
        var result = await context.SaveChangesAsync();
       // if (result==0) return BadRequest(result.Errors);

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        var user = await context.Users
        .Include(p=>p.Photos)
        .FirstAsync(u=>u.UserName== dto.Username);
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
            KnownAs=user.KnownAs,
            Gender=user.Gender,
            Token=tokenService.CreateToken(user),
            PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url
        
            };
    }

    private async Task<bool> UserExists(string UserName)
    {
       return await  context.Users.AnyAsync(u=>u.UserName.ToLower()==UserName.ToLower());
    }
}