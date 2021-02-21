﻿using System;
using AADWebApp.Resolver;
using AADWebApp.Services;
using AADWebAppTests.Resolver;
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

            var rdsConfiguration = _configuration.GetSection("RdsConfiguration");
            var sqlConnectionString = string.Format(
                _configuration.GetConnectionString("SqlConnection"),
                rdsConfiguration.GetValue<string>("RdsName"),
                rdsConfiguration.GetValue<string>("Username"),
                rdsConfiguration.GetValue<string>("Password"),
                "{0}");

            // Add services for testing
            services.AddTransient<ISendEmailService, SendEmailService>();
            services.AddTransient<ISendSmsService, SendSmsService>();
            services.AddTransient<IDatabaseNameResolver, TestDatabaseNameResolver>();
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
        /// <returns></returns>
        protected static T Get<T>() => _testServiceProvider.GetService<T>();
    }
}