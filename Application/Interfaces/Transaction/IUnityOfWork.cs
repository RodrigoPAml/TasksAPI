namespace Application.Interfaces.Transaction
{
    /// <summary>
    /// Transaction control interface
    /// </summary>
    public interface IUnityOfWork
    {
        public Task Begin();
        public Task Save();
        public Task Commit();
        public Task Rollback();
    }
}
