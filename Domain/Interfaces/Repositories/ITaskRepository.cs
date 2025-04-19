using Domain.Enums;
using Domain.Models.Responses;
using Domain.Models.Tasks;
using TaskEntity = Domain.Entities.Task;

namespace Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Task entity
    /// </summary>
    public interface ITaskRepository
    {
        public Task<bool> ExistsByName(string name, int? userId, int? currentId = null);
        public Task<TaskEntity> GetById(int id);
        public Task<List<TaskEntity>> GetByUser(int userId);
        public Task<GetPagedResponse<TaskEntity>> GetPaged(int pageNumber, int pageSize, TasksFilterModel filter, TaskOrderByEnum? ordering);
        public Task Create(TaskEntity task);
        public Task Update(TaskEntity task);
        public Task Delete(int id);
    }
}