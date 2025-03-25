using Infra.Entities;
using Infra.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contexts
{
    /// <summary>
    /// Logging context implementation
    /// </summary>
    public partial class InfraContext : DbContext
    {
        public virtual DbSet<DbLog> Logs { get; set; }
        public virtual DbSet<DbMigration> Migrations { get; set; }

        public InfraContext(DbContextOptions<InfraContext> options) : base(options)
        {
            MigrationsExecutor.Execute(this);   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbLog>(entity =>
            {
                entity.ToTable("Logs");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Message);
                entity.Property(e => e.Details);
                entity.Property(e => e.Type);
                entity.Property(e => e.Date);
                entity.Property(e => e.UserId);
            });

            modelBuilder.Entity<DbMigration>(entity =>
            {
                entity.ToTable("Migrations");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Sql);
                entity.Property(e => e.Date);
            });
        }
    }
}