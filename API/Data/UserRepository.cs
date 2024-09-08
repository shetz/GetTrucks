using API.Data;
using API.DTOS;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    

    public async  Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
      return await  context.Users
        .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
        .ToListAsync();
    }

    
    public async Task<MemberDto?> GetMemberAsync(string username, bool isCurrentUser)
    {
       var allusers= await context.Users
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        
        //  var user=  await context.Users
        //     .Where(x => x.UserName == username)
        //      .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
        //      .SingleOrDefaultAsync();  

        var user=allusers.Find(u=>u.Username==username);

        return user;
    }
   

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByPhotoId(int photoId)
    {
        return await context.Users
            .Include(p => p.Photos)
            .IgnoreQueryFilters()
            .Where(p => p.Photos.Any(p => p.Id == photoId))
            .FirstOrDefaultAsync();
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
            .Include(x => x.Photos)
            .ToListAsync();
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    public async Task<bool> SaveAllAsync()
    {
       int n = await context.SaveChangesAsync();
       if(  n>0) 
        return true;
        
       return false;

    }
}