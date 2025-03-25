namespace Domain.Models.Responses
{
    public class GetAllResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public int Count { get; set; }
    }

    public class GetAllResponse : GetAllResponse<object>
    {
    }
}
