using Microsoft.EntityFrameworkCore;

namespace Infra.Interfaces.Mapping
{
    /// <summary>
    /// Interface for the mapping implementations
    /// </summary>
    public interface IMapping
    {
        public void OnMap(ModelBuilder builder);
    }
}