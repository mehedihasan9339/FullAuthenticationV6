using FullAuthenticationV6.Context;
using FullAuthenticationV6.Data;
using FullAuthenticationV6.Services.Auth;
using FullAuthenticationV6.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MyConnection");
builder.Services.AddDbContext<databaseContext>(option => option.UseSqlServer(connectionString));



builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
		.AddEntityFrameworkStores<databaseContext>()
		.AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAuth, AuthService>();


builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 4;
	options.Password.RequiredUniqueChars = 1;

	// Lockout settings.
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(7);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
	// Cookie settings
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromHours(1);

	options.LoginPath = "/Auth/Auth/Login";
	options.AccessDeniedPath = "/Auth/Auth/AccessDenied";
	options.SlidingExpiration = true;
});

//builder.Services.AddAuthentication(option =>
//{
//	option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//	option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//	option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(option =>
//{
//	option.SaveToken = true;
//	option.RequireHttpsMetadata = false;
//	option.TokenValidationParameters = new TokenValidationParameters()
//	{
//		ValidateIssuer = true,
//		ValidateAudience = true,
//		ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//		ValidAudience = builder.Configuration["JWT:ValidAudience"],
//		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
//	};
//});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}"
	);
});

app.MapRazorPages();

app.Run();
