using FullAuthenticationV6.Context;
using FullAuthenticationV6.Data;
using FullAuthenticationV6.Models;
using FullAuthenticationV6.Services.Auth.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FullAuthenticationV6.Services.Auth
{
    public class AuthService : IAuth
    {
        private readonly databaseContext _context;

        public AuthService(databaseContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetUserByUserName(string username)
        {
            var user = await _context.Users.FindAsync(username);
            return user;
        }

        public async Task<IEnumerable<RoleListViewModel>> RoleList()
        {
            var data = await _context.Roles.AsNoTracking().Select(x => new RoleListViewModel
            {
                roleName = x.Name
            }).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var data = await _context.Users.AsNoTracking().ToListAsync();

            return data;
        }

        public async Task<IEnumerable<RoleAndUsersViewModel>> GetRoleAndUsers()
        {
            var roles = await _context.Roles.AsNoTracking().ToListAsync();

            var userRoles = new List<RoleAndUsersViewModel>();

            foreach (var item in roles)
            {
                userRoles.Add(new RoleAndUsersViewModel
                {
                    roleName = item.Name,
                    users = await (from ur in _context.UserRoles
                                   join u in _context.Users on ur.UserId equals u.Id
                                   where ur.RoleId == item.Id
                                   select u.UserName).ToListAsync()
                });
            }

            return userRoles;
        }
    }
}
