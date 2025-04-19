using MediatR;
using Domain.Base;
using Domain.Utils;
using Domain.Models.Tasks;
using Domain.Models.Responses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using TaskEntity = Domain.Entities.Task;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Tasks.GetPagedTasks
{
    /// <summary>
    /// Handler to get pages tasks
    /// </summary>
    public class GetPagedTasksHandler : IRequestHandler<GetPagedTasksQuery, Result<GetPagedResponse<TaskEntity>>>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ITokenService _tokenService;

        public GetPagedTasksHandler(IServiceProvider provider)
        {
            _taskRepo = provider.GetService<ITaskRepository>();
            _tokenService = provider.GetService<ITokenService>();
        }

        public async Task<Result<GetPagedResponse<TaskEntity>>> Handle(GetPagedTasksQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Result.MakeFailure<GetPagedResponse<TaskEntity>>("Invalid request");

            if(request.Priority != null && !request.Priority.Value.IsInRange())
                return Result.MakeFailure<GetPagedResponse<TaskEntity>>("Invalid priority filter in request");

            if (request.Status != null && !request.Status.Value.IsInRange())
                return Result.MakeFailure<GetPagedResponse<TaskEntity>>("Invalid status filter in request");

            if (request.Ordering != null && !request.Ordering.Value.IsInRange())
                return Result.MakeFailure<GetPagedResponse<TaskEntity>>("Invalid ordering filter in request");

            var userId = _tokenService.GetToken().Id;
            var tasks = await _taskRepo.GetPaged(
                request.PageNumber,
                request.PageSize,
                new TasksFilterModel
                {
                    CategoryId = request.CategoryId,
                    Name = request.Name,
                    Status = request.Status,
                    UserId = userId,
                    Priority = request.Priority,
                },
                request.Ordering
            );

            return Result.MakeSuccess(tasks);
        }
    }
}