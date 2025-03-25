namespace Domain.Utils
{
    public static class EnumUtils
    {
        public static bool IsInRange<T>(this T @enum) where T : Enum
        {
            return Enum.IsDefined(typeof(T), @enum);
        }
    }
}