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
        private BaseEntity _domainRef;

        public void SetDomainRef(BaseEntity domainRef)
        {
            _domainRef = domainRef;
        }   

        public void UpdateDomainId()
        {
            if (_domainRef != null)
            {
                var idProperty = _domainRef
                    .GetType()
                    .GetProperty("Id", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                idProperty?.SetValue(_domainRef, Id);
            }
        }
    }
}
