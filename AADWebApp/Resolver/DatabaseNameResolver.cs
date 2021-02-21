using AADWebApp.Services;

namespace AADWebApp.Resolver
{
    public class DatabaseNameResolver : IDatabaseNameResolver
    {
        public string GetDatabaseName(DatabaseService.AvailableDatabases database)
        {
            return database.ToString();
        }
    }
}