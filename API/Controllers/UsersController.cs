using System.Security.Claims;
using API.Data;
using API.DTOS;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController (IUserRepository userRepo,IMapper mapper) : BaseApiController//DataContext context
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
        var user = await userRepo.GetMemberAsync(username, false);
        //userRepo.GetUserByUsernameAsync(username); //context.Users.Find(id);
        if (user ==null)
            return NotFound();

          //  var userToReturn= mapper.Map<MemberDto>(user);


        return Ok(user);
    }


    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(username==null)
            return BadRequest("Failed to find user");

        var user=await userRepo.GetUserByUsernameAsync(username);
       
        mapper.Map(memberUpdateDto,user);

        if(await userRepo.SaveAllAsync())
            return NoContent();
        //  if(userRepo.)
        //  var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        // if (user == null) return BadRequest("Could not find user");

        // mapper.Map(memberUpdateDto, user);

        // if (await unitOfWork.Complete()) return NoContent();

        return BadRequest("Failed to update the user");
    }

}