using System;
using AADWebApp.Provider;
using AADWebApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AADWebAppTests.Services
{
    [TestClass]
    public class TestBase
    {
        private static IConfiguration _configuration;
        private static IServiceProvider _testServiceProvider;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            // Create a service collection for use in the tests
            IServiceCollection services = new ServiceCollection();

            // Add services for testing
            services.AddTransient<ISendEmailService, SendEmailService>();
            services.AddTransient<ISendSmsService, SendSmsService>();
            services.AddTransient<IDatabaseNameResolver, TestDatabaseNameResolver>();
            services.AddTransient<IDatabaseService, DatabaseService>(serviceProvider =>
                new DatabaseService(
                    _configuration.GetConnectionString("SqlConnection"),
                    serviceProvider.GetService<IDatabaseNameResolver>()
                )
            );

            // Manually build the service provider
            _testServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Get a service of type T from the test service provider.
        /// </summary>
        /// <typeparam name="T">The interface definition of the class to retrieve via the service container.</typeparam>
        /// <returns></returns>
        protected static T Get<T>() => _testServiceProvider.GetService<T>();
    }
}