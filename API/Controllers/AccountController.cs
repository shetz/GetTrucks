// using System.Security.Cryptography;
// using System.Text;
// using API.Data;
// using API.DTOS;
// using API.Entities;
// using AutoMapper;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace API.Controllers;

// public class AccountController(UserManager<AppUser> userManager,
//  ItokenService tokenService,
//  IMapper mapper) : BaseApiController
// {
//     [HttpPost("register")] // account/register
//     public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
//     {
//         if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

//         var user = mapper.Map<AppUser>(registerDto);

//         user.UserName = registerDto.Username.ToLower();

//         var result = await userManager.CreateAsync(user, registerDto.Password);

//         if (!result.Succeeded) return BadRequest(result.Errors);

//         return new UserDto
//         {
//             Username = user.UserName,
//             Token = await tokenService.CreateToken(user),
//             KnownAs = user.KnownAs,
//             Gender = user.Gender
//         };
//     }

//     [HttpPost("login")]
//     public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
//     {
//         var user = await userManager.Users
//             .Include(p => p.Photos)
//                 .FirstOrDefaultAsync(x =>
//                     x.NormalizedUserName == loginDto.Username.ToUpper());

//         if (user == null || user.UserName == null) return Unauthorized("Invalid username");

//         return new UserDto
//         {
//             Username = user.UserName,
//             KnownAs = user.KnownAs,
//             Token = await tokenService.CreateToken(user),
//             Gender = user.Gender,
//             PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
//         };
//     }

//     private async Task<bool> UserExists(string username)
//     {
//         return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper()); // Bob != bob
//     }
// }







using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOS;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(UserManager<AppUser> userManager,
 //DataContext context,
 ItokenService tokenService,
 IMapper mapper) : BaseApiController
{
    [HttpPost("register")] // account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
        using var hmac = new HMACSHA512();

        var user = mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.Username.ToLower();
       // user.PasswordHashed=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
       // user.PasswordSalt=hmac.Key;

        // await context.Users.AddAsync (user);
        // var result = await context.SaveChangesAsync();

        var result = await userManager.CreateAsync(user, registerDto.Password);
        if(!result.Succeeded)
            return BadRequest(result.Errors);

        return new UserDto
        {
            Username = user.UserName,
            Token = await tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        var user = await userManager.Users 
        .Include(p=>p.Photos)
      
       .FirstAsync(u => u.NormalizedUserName == dto.Username.ToUpper());
        
        if (user==null)
            return BadRequest("no username found");

        var result = await userManager.CheckPasswordAsync(user,dto.Password);

        if(!result) return Unauthorized();

        
        
        return new UserDto{
            Username=user.UserName!,
            KnownAs=user.KnownAs,
            Gender=user.Gender,
            Token=await tokenService.CreateToken(user),
            PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url

            };
    }

    private async Task<bool> UserExists(string UserName)
    {
        //    return await  context.Users.AnyAsync(u=>u.NormalizedUserName==UserName.ToUpper());
        return await userManager.Users.AnyAsync(u => u.NormalizedUserName == UserName.ToUpper());
    }
}