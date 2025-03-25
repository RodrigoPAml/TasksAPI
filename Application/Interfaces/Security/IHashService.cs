namespace Application.Interfaces.Security
{
    /// <summary>
    /// Hasher interface
    /// </summary>
    public interface IHashService
    {
        public string Hash(string content);
        public bool IsValid(string content, string hash);
    }
}
