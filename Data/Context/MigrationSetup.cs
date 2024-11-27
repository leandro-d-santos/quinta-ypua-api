using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Context
{
    public static class MigrationSetup
    {
        public static void RunMigrations(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var connection = serviceProvider.GetService<DbConnection>();
            connection.Database.Migrate();
        }
    }
}