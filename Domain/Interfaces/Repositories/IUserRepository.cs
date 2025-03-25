using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for User entity
    /// </summary>
    public interface IUserRepository
    {
        public Task<bool> ExistsByEmail(string email);
        public Task<bool> ExistsById(int id);
        public Task<User> GetByEmail(string email);
        public Task<User> GetById(int id);
        public Task Create(User user);
        public Task UpdatePassword(int id, string password);
        public Task UpdateUsername(int id, string username);
        public Task Delete(int id);
    }
}