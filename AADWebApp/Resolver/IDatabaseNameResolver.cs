using AADWebApp.Services;

namespace AADWebApp.Resolver
{
    public interface IDatabaseNameResolver
    {
        public string GetDatabaseName(DatabaseService.AvailableDatabases database);
    }
}