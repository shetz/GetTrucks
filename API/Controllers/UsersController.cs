using API.Data;
using API.DTOS;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController (IUserRepository userRepo) : BaseApiController//DataContext context
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>>  GetUsers()
    {
        var users =await userRepo .GetMembersAsync(); //userRepo.GetUsersAsync(); //context.Users.ToListAsync();
        //var usersToReturn =mapper.Map<IEnumerable<MemberDto>>(users);
        return Ok(users);
    }

   
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepo.GetMemberAsync(username, false);//userRepo.GetUserByUsernameAsync(username); //context.Users.Find(id);
        if (user ==null)
            return NotFound();

          //  var userToReturn= mapper.Map<MemberDto>(user);


        return Ok(user);
    }
}