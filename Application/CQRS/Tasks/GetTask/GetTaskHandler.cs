using MediatR;
using Domain.Base;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using TaskEntity = Domain.Entities.Task;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Tasks.GetTask
{
    /// <summary>
    /// Handler to get a Task
    /// </summary>
    public class GetTaskHandler : IRequestHandler<GetTaskQuery, Result<TaskEntity>>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ITokenService _tokenService;

        public GetTaskHandler(IServiceProvider provider)
        {
            _taskRepo = provider.GetService<ITaskRepository>();
            _tokenService = provider.GetService<ITokenService>();
        }

        public async Task<Result<TaskEntity>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Result.MakeFailure<TaskEntity>("Invalid request");

            var task = await _taskRepo.GetById(request.Id);

            if (task == null)
                return Result.MakeFailure<TaskEntity>("Task not found");

            var userId = _tokenService.GetToken().Id;

            if (task.UserId != userId)
                return Result.MakeFailure<TaskEntity>("Not allowed");

            return Result.MakeSuccess(task);
        }
    }
}