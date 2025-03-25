namespace Domain.Models.Responses
{
    public class GetPagedResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public int Pages { get; set; }
        public int Count { get; set; }
    }

    public class GetPagedResponse : GetPagedResponse<object>
    {
    }
}
