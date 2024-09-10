using System.Security.Claims;
using API.DTOS;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController (IPhotoService photoService,
 IUserRepository userRepo,IMapper mapper) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>>  GetUsers()
    {
        var users =await userRepo .GetMembersAsync(); 
        return Ok(users);
    }

   
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepo.GetMemberAsync(username, false);
        
         if (user ==null)
            return NotFound();

         

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
      

        return BadRequest("Failed to update the user");
    }

    
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await userRepo.GetUserByUsernameAsync(username);//User.GetUsername());
        
        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo);
        
        if(await userRepo.SaveAllAsync()) //return mapper.Map<PhotoDto>(photo);
            return CreatedAtAction(nameof(GetUser),
                 new { username = user.UserName }, mapper.Map<PhotoDto>(photo));


        // if (await unitOfWork.Complete())
        //     return CreatedAtAction(nameof(GetUser),
        //         new { username = user.UserName }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

}