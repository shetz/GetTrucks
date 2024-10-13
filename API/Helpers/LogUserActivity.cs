using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;


        var userId = resultContext.HttpContext.User.GetUserId();
        //var userName = resultContext.HttpContext.User.GetUsername();
        var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
         var user = await repo.GetUserByIdAsync(userId);//GetUserByUsernameAsync(userName);
             if(user==null)
             return;

         user.LastActive= DateTime.UtcNow;
         await repo.SaveAllAsync();
        
    }
}