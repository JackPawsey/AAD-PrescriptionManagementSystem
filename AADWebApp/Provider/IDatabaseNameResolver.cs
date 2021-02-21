using AADWebApp.Services;

namespace AADWebApp.Provider
{
    public interface IDatabaseNameResolver
    {
        public string GetDatabaseName(DatabaseService.AvailableDatabases database);
    }
}