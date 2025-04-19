namespace Api.Base
{
    /// <summary>
    /// Base response for the API
    /// </summary>
    public class BaseResponse
    {
        public object Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int Code { get; set; }
    }

    /// <summary>
    /// Base response for the API
    /// </summary>
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int Code { get; set; }
    }
}