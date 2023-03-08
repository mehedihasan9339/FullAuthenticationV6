using FullAuthenticationV6.Data;
using FullAuthenticationV6.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullAuthenticationV6.Context
{
	public class databaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public databaseContext(DbContextOptions<databaseContext> options, IHttpContextAccessor _httpContextAccessor) : base(options)
		{
			this._httpContextAccessor = _httpContextAccessor;
			Database.SetCommandTimeout(250000);
		}
	}
}
