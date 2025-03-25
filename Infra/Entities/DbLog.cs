using Infra.Base;

namespace Infra.Entities
{
    /// <summary>
    /// Log entity for database
    /// </summary>
    public class DbLog : BaseDbEntity
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public int Type { get; set; }
        public DateTime Date {  get; set; } 
        public int? UserId { get; set; }
    }
}