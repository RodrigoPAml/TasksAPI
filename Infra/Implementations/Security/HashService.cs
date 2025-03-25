using Application.Interfaces.Security;
using static BCrypt.Net.BCrypt;

namespace Infra.Implementations.Security
{
    /// <summary>
    /// Hashing implementation, usually used for password hashing
    /// </summary>
    public class HashService : IHashService
    {
        private static int WorkFactor = 12;

        public string Hash(string content)
        {
            return HashPassword(content, WorkFactor);
        }

        public bool IsValid(string content, string hash)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(hash))
                return false;

            return Verify(content, hash);
        }
    }
}
