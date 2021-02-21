using AADWebApp.Services;

namespace AADWebApp.Provider
{
    public class DatabaseNameResolver : IDatabaseNameResolver
    {
        public string GetDatabaseName(DatabaseService.AvailableDatabases database)
        {
            return database.ToString();
        }
    }
}