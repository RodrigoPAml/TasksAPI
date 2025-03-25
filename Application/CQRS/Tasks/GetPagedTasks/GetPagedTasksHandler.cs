using MediatR;
using Domain.Base;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using TaskEntity = Domain.Entities.Task;
using Microsoft.Extensions.DependencyInjection;
using Domain.Models.Responses;

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

            var userId = _tokenService.GetToken().Id;
            var tasks = await _taskRepo.GetPaged(userId, request.PageNumber, request.PageSize);

            return Result.MakeSuccess(tasks);
        }
    }
}