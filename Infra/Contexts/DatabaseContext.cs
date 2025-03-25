using Infra.Base;
using Infra.Entities;
using Infra.Interfaces.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contexts
{
    /// <summary>
    /// Database context implementation
    /// </summary>
    public partial class DatabaseContext : DbContext
	{
        public virtual DbSet<DbUser> Users { get; set; }
        public virtual DbSet<DbCategory> Categories { get; set; }
        public virtual DbSet<DbTask> Tasks { get; set; }
        public virtual DbSet<DbVerificationCode> VerificationCodes { get; set; }

        private readonly IEnumerable<IMapping> _mappings;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IEnumerable<IMapping> mappings) : base(options)
        {
            _mappings = mappings;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var mapping in _mappings)
                mapping.OnMap(modelBuilder);
        }

        public override int SaveChanges()
        {
            var ret = base.SaveChanges();

            foreach (var entry in ChangeTracker.Entries<BaseDbEntity>())
                entry.Entity.UpdateDomainId();

            return ret;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var ret = await base.SaveChangesAsync(cancellationToken);

            foreach (var entry in ChangeTracker.Entries<BaseDbEntity>())
                entry.Entity.UpdateDomainId();

            return ret;
        }
    }
}