using Infra.Entities;
using Infra.Interfaces.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infra.Mappings
{
    /// <summary>
    /// Mapping for the User entity
    /// </summary>
    public class UserMapping : IMapping
    {
        public void OnMap(ModelBuilder builder)
        {
            builder.Entity<DbUser>(entity =>
            {
                entity.ToTable("Users");
            
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username);
                entity.Property(e => e.Password);
                entity.Property(e => e.Email);
                entity.Property(e => e.Profile);
            });
        }
    }
}
