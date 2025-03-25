using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Infra.Base
{
    /// <summary>
    /// The base database entity
    /// </summary>
    public class BaseDbEntity
    {
        public int Id { get; set; }

        [NotMapped]
        public BaseEntity DomainRef { get; set; }

        public void UpdateDomainId()
        {
            if (DomainRef != null)
            {
                var idProperty = DomainRef
                    .GetType()
                    .GetProperty("Id", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                idProperty?.SetValue(DomainRef, Id);
            }
        }
    }
}
