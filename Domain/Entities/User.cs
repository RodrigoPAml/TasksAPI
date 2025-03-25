using Domain.Base;
using Domain.Enums;
using Domain.Utils;

namespace Domain.Entities
{
    /// <summary>
    /// User entity
    /// </summary>
    public sealed class User : BaseEntity
    {
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string HashedPassword { get; private set; }
        public ProfileTypeEnum Profile { get; private set; }

        private User() {}

        private User(
            string email,
            string username,
            string password,
            string hashedPassword,
            ProfileTypeEnum profile
        )
        {
            Email = email;
            Username = username;
            Password = password;
            HashedPassword = hashedPassword;
            Profile = profile;
        }

        public static Result<User> Create(
            string email,
            string username,
            string password,
            string hashedPassword,
            ProfileTypeEnum profile
        )
        {
            var user = new User(email, username, password, hashedPassword, profile);
            var result = ValidateAll(user);

            if (!result.Success)
                return Result.MakeFailure<User>(result.Message);

            return Result.MakeSuccess(user);
        }

        public Operation ChangePassword(string newPassword, string hashedPassword)
        {
            var validation = ValidatePassword(newPassword);

            if (!validation.Success)
                return validation;

            Password = newPassword;
            HashedPassword = hashedPassword;

            return Operation.MakeSuccess();
        }

        public Operation ChangeUsername(string username)
        {
            var error = ValidateUsername(username);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            Username = username;
            return Operation.MakeSuccess();
        }

        private static Operation ValidateAll(User user)
        {
            var errorUsername = ValidateUsername(user.Username);

            if (!string.IsNullOrEmpty(errorUsername))
                return Operation.MakeFailure(errorUsername);

            var emailValidation = ValidateEmail(user.Email);

            if (!emailValidation.Success)
                return emailValidation;

            var passwordValidation = ValidatePassword(user.Password);

            if (!passwordValidation.Success)
                return passwordValidation;

            var profileValidation = ValidateProfile(user.Profile);

            if (!profileValidation.Success)
                return profileValidation;

            return Operation.MakeSuccess();
        }

        private static string ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return "Username cannot be empty";

            if (username.Length < 4)
                return "Username must be at least 4 characters long";

            if (username.Length > 64)
                return "Username must be no more than 64 characters long";

            return string.Empty;
        }

        private static Operation ValidateProfile(ProfileTypeEnum profile)
        {
            return !profile.IsInRange() 
                ? Operation.MakeFailure("Invalid profile provided")
                : Operation.MakeSuccess();
        }

        private static Operation ValidatePassword(string password)
        {
            return PasswordUtils.ValidatePassword(password);
        }

        private static Operation ValidateEmail(string email)
        {
            return EmailUtils.ValidateEmail(email);
        }
    }
}