using Infra.Entities;
using Infra.Interfaces.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infra.Mappings
{
    /// <summary>
    /// Mapping for the Category entity
    /// </summary>
    public class CategoryMapping : IMapping
    {
        public void OnMap(ModelBuilder builder)
        {
            builder.Entity<DbCategory>(entity =>
            {
                entity.ToTable("Categories");
            
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name);
                entity.Property(e => e.UserId);

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Categories)
                    .HasForeignKey(e => e.UserId);
            });
        }
    }
}
