using Configuration.Configuration;
using Data.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public sealed class DbConnection : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IAppSettings _appSettings;

        public DbConnection(IConfiguration configuration,
                            IAppSettings appSettings,
                            DbContextOptions options) : base(options)
        {
            _configuration = configuration;
            _appSettings = appSettings;
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            AddSoftDelete();
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionStringName = _appSettings.ConnectionStringName;
            if (connectionStringName == null)
            {
                throw new ArgumentNullException(nameof(connectionStringName));
            }
            string? connectionString = _configuration.GetConnectionString(connectionStringName);
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbConnection).Assembly);
        }

        private void AddSoftDelete()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity && e.State == EntityState.Deleted);
            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;
                entity.State = EntityState.Modified;
                ((Entity)entity.Entity).DeletedAt = now;
            }
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