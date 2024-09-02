using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class BuggyController(DataContext context) : BaseApiController
{
    
    [HttpGet("Auth")]
    [Authorize]
    public ActionResult<string>  GetAuth()
    {
       
        return "secret auth";
    }

    
    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var user =context.Users.Find(-1);
        if(user==null)
        return NotFound();

        return user;
    }
    
    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        var user = context.Users.Find(-1)??throw new Exception("some nasty error");
        return "bad";
    }
    
    

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {

        return BadRequest();
    }

    
}