using Infra.Base;

namespace Infra.Entities
{
    /// <summary>
    /// Verification Code entity for database
    /// </summary>
    public class DbVerificationCode : BaseDbEntity
    {
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public int Code  { get; set; }
        public int Type { get; set; }
    }
}