using AADWebApp.Resolver;
using AADWebApp.Services;

namespace AADWebAppTests.Resolver
{
    public class TestDatabaseNameResolver : IDatabaseNameResolver
    {
        public string GetDatabaseName(DatabaseService.AvailableDatabases database)
        {
            return database + "_test";
        }
    }
}