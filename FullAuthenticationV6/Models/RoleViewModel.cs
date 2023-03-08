namespace FullAuthenticationV6.Models
{
    public class RoleViewModel
    {
        public string roleName { get; set; }

        public IEnumerable<RoleListViewModel> roleList { get; set; }
    }
}
