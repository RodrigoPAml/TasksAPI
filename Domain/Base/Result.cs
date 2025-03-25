namespace Domain.Base
{
    /// <summary>
    /// Generic class that represents an operation with a result.
    /// </summary>
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Content { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// The static class for factory methods
    /// </summary>
    public static class Result
    {
        public static Result<T> MakeSuccess<T>(T content, string message = "")
        {
            return new Result<T>()
            {
                Success = true,
                Content = content,
                Message = message
            };
        }

        public static Result<T> MakeFailure<T>(string message = "")
        {
            return new Result<T>()
            {
                Success = false,
                Content = default,
                Message = message
            };
        }
    }
}
