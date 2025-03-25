using Domain.Base;

namespace Domain.Entities
{
    /// <summary>
    /// Category entity
    /// </summary>
    public sealed class Category : BaseEntity
    {
        public string Name { get; private set; }
        public int UserId { get; private set; }

        private Category() { }

        private Category(string name, int userId)
        {
            Name = name;
            UserId = userId;
        }

        public static Result<Category> Create(string name, int userId)
        {
            var result = ValidateAll(name, userId);

            if (!result.Success)
                return Result.MakeFailure<Category>(result.Message);

            var category = new Category(name, userId);

            return Result.MakeSuccess(category);
        }

        public Operation ChangeName(string name)
        {
            var error = ValidateName(name);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            Name = name;

            return Operation.MakeSuccess();
        }

        private static Operation ValidateAll(string name, int userId)
        {
            var error = ValidateName(name);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            error = ValidateUserId(userId);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            return Operation.MakeSuccess();
        }

        private static string ValidateUserId(int userId)
        {
            if (userId <= 0)
                return "Category with invalid user";

            return string.Empty;
        }

        private static string ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "Username cannot be empty";

            if (name.Length < 1)
                return "Username must be at least 1 character long";

            if (name.Length > 128)
                return "Username must be no more than 128 characters long";

            return string.Empty;
        }
    }
}
