using System.Data.Entity;

namespace EFMigrations.DataAccess.ClientMigrations
{
    public class ClientSpecificMigrationContext : DbContext
    {
        public ClientSpecificMigrationContext() : base("name=EfMigrationContext")
        {
        }
    }
}
