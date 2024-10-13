using API.Data;
using API.DTOS;
using API.Entities;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    

    public async  Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
      var query=  context.Users.AsQueryable();
       query=query.Where(m=> m.UserName != userParams.CurrentUsername);

        if(userParams.Gender!=null)
            query = query.Where(m => m.Gender == userParams.Gender);

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };
        
        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider),
        userParams.PageNumber,userParams.PageSize);
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
        return await context.Users//.FindAsync(id);
             .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.Id == id);
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

    public Task<Photo> GetPhotoById(int photoId)
    {
        throw new NotImplementedException();
    }
}