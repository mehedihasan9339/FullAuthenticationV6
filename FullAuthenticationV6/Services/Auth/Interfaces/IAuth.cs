using FullAuthenticationV6.Data;
using FullAuthenticationV6.Models;

namespace FullAuthenticationV6.Services.Auth.Interfaces
{
    public interface IAuth
    {
        Task<ApplicationUser> GetUserByUserName(string username);
        Task<IEnumerable<RoleListViewModel>> RoleList();
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<IEnumerable<RoleAndUsersViewModel>> GetRoleAndUsers();
	}
}
