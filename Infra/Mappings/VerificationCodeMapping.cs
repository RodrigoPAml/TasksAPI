using Infra.Entities;
using Infra.Interfaces.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infra.Mappings
{
    /// <summary>
    /// Mapping for the Verification Code entity
    /// </summary>
    public class VerificationCodeMapping : IMapping
    {
        public void OnMap(ModelBuilder builder)
        {
            builder.Entity<DbVerificationCode>(entity =>
            {
                entity.ToTable("VerificationCodes");
            
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Email);
                entity.Property(e => e.Date);
                entity.Property(e => e.Code);
                entity.Property(e => e.Type);
            });
        }
    }
}
