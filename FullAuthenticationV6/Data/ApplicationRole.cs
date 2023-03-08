using Microsoft.AspNetCore.Identity;

namespace FullAuthenticationV6.Data
{
	public class ApplicationRole : IdentityRole
	{
		public ApplicationRole() : base() { }
		public ApplicationRole(string roleName) : base(roleName)
		{
		}
	}
}
