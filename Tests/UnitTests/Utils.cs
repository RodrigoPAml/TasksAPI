using System.Reflection;

namespace UnitTests
{
    /// <summary>
    /// Utils class for tests
    /// </summary>
    public static class Utils
    {
        public static string GenerateRandomString(int length)
        {
            if (length < 0)
                return null;

            if(length == 0)
                return string.Empty;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void InjectValue<T>(T target, string fieldName, object value)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("Field name cannot be null or empty", nameof(fieldName));

            var type = target.GetType();
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (field != null)
            {
                field.SetValue(target, value);
                return;
            }

            var property = type.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (property?.SetMethod != null)
            {
                property.SetValue(target, value);
                return;
            }

            throw new ArgumentException($"Field or property '{fieldName}' not found or not settable in type '{type.FullName}'", nameof(fieldName));
        }
    }
}
