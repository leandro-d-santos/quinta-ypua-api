using Configuration.Configuration;
using Data.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public sealed class DbConnection : DbContext
    {
        //private readonly IConfiguration _configuration;
        //private readonly IAppSettings _appSettings;

        //public DbConnection(IConfiguration configuration,
        //                    IAppSettings appSettings,
        //                    DbContextOptions options) : base(options)
        //{
        //    _configuration = configuration;
        //    _appSettings = appSettings;
        //}

        //public DbConnection(DbContextOptions options) : base(options)
        //{
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=dev_quinta_ypua;Username=postgres;Password=Santos$1001");
            optionsBuilder.UseSqlite("Data Source=sqlite.db");
            //string? connectionStringName = _appSettings.ConnectionStringName;
            //if (connectionStringName == null)
            //{
            //    throw new ArgumentNullException(nameof(connectionStringName));
            //}
            //string? connectionString = _configuration.GetConnectionString(connectionStringName);
            //if (connectionString == null)
            //{
            //    throw new ArgumentNullException(nameof(connectionString));
            //}

            //if (connectionString.StartsWith("Data Source="))
            //{
            //    optionsBuilder.UseSqlite(connectionString);
            //}
            //else
            //{
            //    optionsBuilder.UseNpgsql(connectionString);
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbConnection).Assembly);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;
                if (entity.State == EntityState.Added)
                {
                    ((Entity)entity.Entity).CreatedAt = now;
                }
                else if (entity.State == EntityState.Modified)
                {
                    ((Entity)entity.Entity).UpdatedAt = now;
                }
            }
        }
    }
}