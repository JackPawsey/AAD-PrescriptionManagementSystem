using AADWebApp.Resolver;
using AADWebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AADWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var rdsConfiguration = Configuration.GetSection("RdsConfiguration");
            var sqlConnectionString = string.Format(
                Configuration.GetConnectionString("SqlConnection"),
                rdsConfiguration.GetValue<string>("RdsName"),
                rdsConfiguration.GetValue<string>("Username"),
                rdsConfiguration.GetValue<string>("Password"),
                "{0}");

            services.AddRazorPages()
                    .AddRazorRuntimeCompilation();
            services.AddTransient<ISendEmailService, SendEmailService>();
            services.AddTransient<ISendSmsService, SendSmsService>();
            services.AddTransient<IDatabaseNameResolver, DatabaseNameResolver>();
            services.AddTransient<IDatabaseService, DatabaseService>(serviceProvider =>
                new DatabaseService(
                    sqlConnectionString,
                    serviceProvider.GetService<IDatabaseNameResolver>()
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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
                endpoints.MapRazorPages();
            });
        }
    }
}
