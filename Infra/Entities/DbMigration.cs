using Infra.Base;

namespace Infra.Entities
{
    /// <summary>
    /// Log entity for database
    /// </summary>
    public class DbMigration : BaseDbEntity
    {
        public string Sql { get; set; }
        public DateTime Date {  get; set; } 
    }
}