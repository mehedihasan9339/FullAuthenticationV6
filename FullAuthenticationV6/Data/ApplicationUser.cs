using Microsoft.AspNetCore.Identity;

namespace FullAuthenticationV6.Data
{
	public class ApplicationUser:IdentityUser
	{
		public int? isActive { get; set; } = 1;
	}
}
