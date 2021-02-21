using AADWebApp.Services;

namespace AADWebApp.Provider
{
    public class TestDatabaseNameResolver : IDatabaseNameResolver
    {
        public string GetDatabaseName(DatabaseService.AvailableDatabases database)
        {
            return database + "_test";
        }
    }
}