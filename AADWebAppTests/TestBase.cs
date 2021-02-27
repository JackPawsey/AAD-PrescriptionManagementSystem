using System;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Resolver;
using AADWebApp.Services;
using AADWebAppTests.Resolver;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

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

            var rdsConfiguration = _configuration.GetSection("RdsConfiguration");
            var sqlConnectionString = string.Format(
                _configuration.GetConnectionString("SqlConnection"),
                rdsConfiguration.GetValue<string>("RdsName"),
                rdsConfiguration.GetValue<string>("Username"),
                rdsConfiguration.GetValue<string>("Password"),
                "{0}");

            // Add services for testing
            services.AddSingleton<INotificationScheduleService, NotificationScheduleService>();

            services.AddScoped<INotificationService, NotificationService>();

            services.AddTransient<ISendEmailService, SendEmailService>();
            services.AddTransient<ISendSmsService, SendSmsService>();
            services.AddTransient<IBloodTestService, BloodTestService>();
            services.AddTransient<IMedicationService, MedicationService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IPrescriptionService, PrescriptionService>();
            services.AddTransient<IPrescriptionCollectionService, PrescriptionCollectionService>();
            services.AddTransient<IPrescriptionSchedule, PrescriptionSchedule>();
            services.AddTransient<IDatabaseNameResolver, TestDatabaseNameResolver>();
            services.AddTransient(serviceProvider => MockUserManager<ApplicationUser>().Object);
            services.AddTransient<IDatabaseService, DatabaseService>(serviceProvider =>
                new DatabaseService(
                    sqlConnectionString,
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
        /// <returns>The registered service implementation for the given interface.</returns>
        protected static T Get<T>() => _testServiceProvider.GetService<T>();

        /// <summary>
        /// Serialize the provided input string, applying data formatting.
        /// </summary>
        /// <param name="input">The string to serialize.</param>
        /// <returns>The serialized string.</returns>
        protected static string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });
        }

        /// <summary>
        /// Creates a mock user manager instance for use within testing.
        /// </summary>
        /// <typeparam name="TUser">The type of user the user manage will be dealing with.</typeparam>
        /// <returns>The mocked user manager instance.</returns>
        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var mockUserStore = new Mock<IUserStore<TUser>>();
            var mockUserManager = new Mock<UserManager<TUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser
            {
                FirstName = "firstName",
                LastName = "lastName",
                Email = "cloudcrusaderssystems+unittests@gmail.com",
                PhoneNumber = "+4411234567890"
            } as TUser);

            return mockUserManager;
        }
    }
}