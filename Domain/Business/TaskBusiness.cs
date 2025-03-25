using Domain.Base;
using Domain.Models.Tasks;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using TaskEntity = Domain.Entities.Task;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Business
{
    /// <summary>
    /// Business service implementation for Task entity
    /// </summary>
    public class TaskBusiness : BaseBusiness, ITaskBusiness
    {
        public TaskBusiness(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<Result<TaskEntity>> Create(CreateTaskModel model)
        {
            if (model == null)
                return Result.MakeFailure<TaskEntity>("Model is null");

            var taskResult = TaskEntity.Create(
                model.Name, 
                model.Description,
                model.Status,
                model.Priority,
                model.DueDate,
                model.UserId,
                model.CategoryId
           );

            if (!taskResult.Success)
                return taskResult;

            var repoUser = _provider.GetService<IUserRepository>();

            if (!await repoUser.ExistsById(model.UserId))
                return Result.MakeFailure<TaskEntity>("User not found for task");

            var repoCategory = _provider.GetService<ICategoryRepository>();

            if (!await repoCategory.ExistsById(model.CategoryId, model.UserId))
                return Result.MakeFailure<TaskEntity>("Category not found for task");

            var repoTask = _provider.GetService<ITaskRepository>();

            if (await repoTask.ExistsByName(model.Name, model.UserId))
                return Result.MakeFailure<TaskEntity>("Task with this name already exists");

            return Result.MakeSuccess(taskResult.Content);
        }

        public async Task<Operation> Update(UpdateTaskModel model)
        {
            if (model == null || model.Info == null)
                return Operation.MakeFailure("Model is null");

            if (model.Task == null)
                return Operation.MakeFailure("Task is null");

            var validation = model.Task.Update(
                model.Info.Name,
                model.Info.Description,
                model.Info.Status,
                model.Info.Priority,
                model.Info.DueDate,
                model.Info.CategoryId
            );

            if (!validation.Success)
                return validation;

            var repoCategory = _provider.GetService<ICategoryRepository>();

            if (!await repoCategory.ExistsById(model.Info.CategoryId, model.Task.UserId))
                return Operation.MakeFailure("Category not found for task");

            var repoTask = _provider.GetService<ITaskRepository>();

            if (await repoTask.ExistsByName(model.Info.Name, model.Task.UserId, model.Task.Id))
                return Operation.MakeFailure("Task with this name already exists");

            return Operation.MakeSuccess();
        }
    }
}
