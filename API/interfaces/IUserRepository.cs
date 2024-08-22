// Purpose: Interface for the User Repository.
// This interface is used to define the methods that the User Repository must implement.
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        //Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string username);

        Task<string> GetUserGender(string username);
        
    }
}