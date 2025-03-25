using Infra.Entities;
using Infra.Interfaces.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infra.Mappings
{
    /// <summary>
    /// Mapping for the Task entity
    /// </summary>
    public class TaskMapping : IMapping
    {
        public void OnMap(ModelBuilder builder)
        {
            builder.Entity<DbTask>(entity =>
            {
                entity.ToTable("Tasks");
            
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name);
                entity.Property(e => e.Description);
                entity.Property(e => e.Status);
                entity.Property(e => e.Priority);
                entity.Property(e => e.DueDate);
                entity.Property(e => e.CategoryId);
                entity.Property(e => e.UserId);

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Tasks)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Category)
                  .WithMany(e => e.Tasks)
                  .HasForeignKey(e => e.CategoryId);
            });
        }
    }
}
