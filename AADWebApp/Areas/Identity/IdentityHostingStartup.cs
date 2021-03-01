using System;
using AADWebApp.Areas.Identity;
using AADWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace AADWebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var rdsConfiguration = context.Configuration.GetSection("RdsConfiguration");
                var sqlConnectionString = string.Format(
                    context.Configuration.GetConnectionString("AuthDbContextConnection"),
                    rdsConfiguration.GetValue<string>("RdsName"),
                    rdsConfiguration.GetValue<string>("Username"),
                    rdsConfiguration.GetValue<string>("Password"));

                services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(sqlConnectionString));

                services
                    .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) // Set true to require email confirmation
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