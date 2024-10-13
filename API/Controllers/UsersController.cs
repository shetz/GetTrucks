using System.Security.Claims;
using API.DTOS;
using API.Entities;
using API.Extensions;
using API.Helpers;
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
    public async Task<ActionResult<IEnumerable<MemberDto>>>  GetUsers([FromQuery] UserParams userParams)
    {
        //userParams.CurrentUsername= User.GetUsername();
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (username == null)
            return BadRequest("Failed to find user!!");
        userParams.CurrentUsername = username;

                var users =await userRepo .GetMembersAsync(userParams); 
        Response.AddPaginationHeader(users);// this functionality was add to Response object
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
        var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userID == null)
            return BadRequest("Failed to find user");

        var user=await userRepo.GetUserByIdAsync(int.Parse(userID));  

        mapper.Map(memberUpdateDto,user);

        if(await userRepo.SaveAllAsync())
            return NoContent();
      

        return BadRequest("Failed to update the user");
    }

    
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userID == null)
            return BadRequest("Failed to find user");

        var user = await userRepo.GetUserByIdAsync(int.Parse(userID));

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

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userID == null)
            return BadRequest("Failed to find user");

        var user = await userRepo.GetUserByIdAsync(int.Parse(userID));
        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;
        if (await userRepo.SaveAllAsync()) return NoContent();
        // if (await unitOfWork.Complete()) return NoContent();

        return BadRequest("Problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userID == null)
            return BadRequest("Failed to find user");

        var user = await userRepo.GetUserByIdAsync(int.Parse(userID));

        if (user == null) return BadRequest("User not found");

        var photo = user.Photos.FirstOrDefault(x => x.Id==photoId);
       
        if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

        if (photo.PublicId != null)
        {
            if(photo.PublicId.Length > 0)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
           
        }

        user.Photos.Remove(photo);

        //if (await unitOfWork.Complete()) return Ok();
        if (await userRepo.SaveAllAsync()) return NoContent();
        return BadRequest("Problem deleting photo");
    }

}