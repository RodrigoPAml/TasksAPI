using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Models.Tasks;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Tasks.DeleteTask
{
    /// <summary>
    /// Handler for Task update
    /// </summary>
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, Operation>
    {
        private readonly ITaskRepository _taskRepo;

        private readonly ITokenService _tokenService;
        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public DeleteTaskHandler(IServiceProvider provider)
        {
            _taskRepo = provider.GetService<ITaskRepository>();

            _tokenService = provider.GetService<ITokenService>();
            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var userId = _tokenService.GetToken().Id;

            var deleteModel = new DeleteTaskModel()
            {
                Task = await _taskRepo.GetById(request.Id)
            };

            if(deleteModel.Task != null && deleteModel.Task.UserId != userId)
                return Operation.MakeFailure("Not Allowed");

            await _uow.Begin();

            await _taskRepo.Delete(deleteModel.Task.Id);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}