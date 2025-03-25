namespace Domain.Base
{
    /// <summary>
    /// Generic class that represents an operation with no result.
    /// </summary>
    public class Operation
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static Operation MakeSuccess(string message = "")
        {
            return new Operation()
            {
                Success = true,
                Message = message
            };
        }

        public static Operation MakeFailure(string message = "")
        {
            return new Operation()
            {
                Success = false,
                Message = message
            };
        }
    }
}
