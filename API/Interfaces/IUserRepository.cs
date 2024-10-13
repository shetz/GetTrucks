using API.DTOS;
using API.Entities;
using API.Helpers;
//using API.Helpers;

namespace API;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    
    Task<AppUser?> GetUserByPhotoId(int photoId);

    //Task<IEnumerable<MemberDto>> GetMembersAsync();

    Task<MemberDto?> GetMemberAsync(string username, bool isCurrentUser);
    Task<bool> SaveAllAsync();
    Task<Photo> GetPhotoById(int photoId);
}