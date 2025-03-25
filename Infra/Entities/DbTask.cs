using Infra.Base;

namespace Infra.Entities
{
    /// <summary>
    /// Task entity for database
    /// </summary>
    public class DbTask : BaseDbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public DbUser User { get; set; }
        public DbCategory Category { get; set; }
    }
}
