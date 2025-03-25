using Domain.Base;

namespace Domain.Utils
{
    /// <summary>
    /// Utility for passwords
    /// </summary>
    public static class PasswordUtils
    {
        public static Operation ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return Operation.MakeFailure("Password cannot be empty");

            if (password.Length < 10)
                return Operation.MakeFailure("Password must be at least 10 characters long");

            if (password.Length > 32)
                return Operation.MakeFailure("Password must be no more than 32 characters long");

            return Operation.MakeSuccess();
        }
    }
}
