using FullAuthenticationV6.Controllers;
using FullAuthenticationV6.Data;
using FullAuthenticationV6.Models;
using FullAuthenticationV6.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace FullAuthenticationV6.Areas.Auth.Controllers
{
    [Area("Auth")]
	[AllowAnonymous]
    public class AuthController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly IAuth _authService;

		public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IAuth authService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_authService = authService;
		}

		[HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
			var userIsExist = await _userManager.FindByNameAsync(model.userName);

			if (userIsExist != null)
			{
				return RedirectToAction("Login");
			}

			var user = new ApplicationUser
			{
				UserName = model.userName,
				Email = model.email,
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var result = await _userManager.CreateAsync(user, model.password);

			if (!result.Succeeded)
			{
				return RedirectToAction("Login");
			}
			else
			{
				return RedirectToAction("Login");
			}
        }

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.userName);
				if (user != null)
				{
					if (user.isActive == 1)
					{
						try
						{
							var result = await _signInManager.PasswordSignInAsync(model.userName, model.password, isPersistent: false, lockoutOnFailure: false);
							if (result.Succeeded)
							{
								if (returnUrl != null)
								{
									return RedirectToLocal(returnUrl);
								}
								else
								{
									return Redirect("/Home/Index");
								}
							}
							else
							{
								ModelState.AddModelError(string.Empty, "Invalid login attempt.");
								return View(model);
							}
						}
						catch (Exception ex)
						{
							throw;
						}
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Invalid login attempt.");
						return View(model);
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View(model);
				}
			}

			return RedirectToAction("Login");
		}


		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
		}


		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Login");
		}



		public async Task<IActionResult> CreateRole()
		{
			var data = new RoleViewModel
			{
				roleList = await _authService.RoleList()
			};
			return View(data);
		}

		[HttpPost]
		public async Task<IActionResult> CreateRole(RoleViewModel model)
		{
			var roleExist = await _roleManager.RoleExistsAsync(model.roleName);

			if (!roleExist)
			{
				await _roleManager.CreateAsync(new ApplicationRole(model.roleName));
			}

			return RedirectToAction("CreateRole");
		}

		[Authorize]
		public async Task<IActionResult> AssignRole()
		{
			var data = new AssignRoleViewModel
			{
				roles = await _authService.RoleList(),
				users = await _authService.GetAllUsers(),
				roleUsers = await _authService.GetRoleAndUsers()
			};
			return View(data);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
		{
			var user = await _userManager.FindByNameAsync(model.userName);
			var isInRole = await _userManager.IsInRoleAsync(user, model.roleName);

			if (!isInRole)
			{
				await _userManager.AddToRoleAsync(user, model.roleName);
			}

			return RedirectToAction("AssignROle");
		}

	}
}
