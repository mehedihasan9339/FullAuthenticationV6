using FullAuthenticationV6.Data;

namespace FullAuthenticationV6.Models
{
    public class AssignRoleViewModel
    {
        public string roleName { get; set; }
        public string userName { get; set; }

        public IEnumerable<RoleListViewModel> roles { get; set; }
        public IEnumerable<ApplicationUser> users { get; set; }
        public IEnumerable<RoleAndUsersViewModel> roleUsers { get; set; }
    }
}
