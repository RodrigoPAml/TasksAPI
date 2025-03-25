using Domain.Base;
using System.Text.RegularExpressions;

namespace Domain.Utils
{
    public static class EmailUtils
    {
        public static Operation ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Operation.MakeFailure("Email cannot be empty");

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!emailRegex.IsMatch(email))
                return Operation.MakeFailure("Invalid email format");

            if (email.Length > 128)
                return Operation.MakeFailure("Email must be no more than 128 characters long");

            return Operation.MakeSuccess();
        }
    }
}