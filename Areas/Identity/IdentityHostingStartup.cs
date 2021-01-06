using System;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(AADWebApp.Areas.Identity.IdentityHostingStartup))]
namespace AADWebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthDbContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) // Set true to require email confirmation
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AuthDbContext>();

                services.ConfigureApplicationCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/Identity/Account/Login";
                    options.SlidingExpiration = true; // Instruct the handler to re-issue a new cookie with a new expiration time any time it processes a request which is more than halfway through the expiration window.
                });
            });
        }
    }
}