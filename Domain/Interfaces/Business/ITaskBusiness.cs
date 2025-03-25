using Domain.Base;
using Domain.Models.Tasks;
using TaskEntity = Domain.Entities.Task;

namespace Domain.Interfaces.Business
{
    /// <summary>
    /// Business Service interface for Task entity
    /// </summary>
    public interface ITaskBusiness
    {
        public Task<Result<TaskEntity>> Create(CreateTaskModel model);
        public Task<Operation> Update(UpdateTaskModel model);
    }
}
