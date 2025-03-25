using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Category entity
    /// </summary>
    public interface ICategoryRepository
    {
        public Task<bool> ExistsByName(string name, int? userId = null, int? currentId = null);
        public Task<bool> IsUsedInTasks(int id);
        public Task<Category> GetById(int id);
        public Task<bool> ExistsById(int id, int? userId = null);
        public Task<List<Category>> GetByUser(int userId);
        public Task Create(Category category);
        public Task Update(Category category);
        public Task Delete(int id);
    }
}