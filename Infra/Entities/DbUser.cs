using Infra.Base;

namespace Infra.Entities
{
    /// <summary>
    /// User entity for database
    /// </summary>
    public class DbUser : BaseDbEntity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Profile { get; set; }
        public ICollection<DbCategory> Categories { get; set; }
        public ICollection<DbTask> Tasks { get; set; }
    }
}