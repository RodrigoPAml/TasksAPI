using Infra.Base;

namespace Infra.Entities
{
    /// <summary>
    /// Category entity for database
    /// </summary>
    public class DbCategory : BaseDbEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }    
        public DbUser User { get; }
        public ICollection<DbTask> Tasks { get; }
    }
}
