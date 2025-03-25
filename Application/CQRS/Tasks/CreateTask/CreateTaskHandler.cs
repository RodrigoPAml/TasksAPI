using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Models.Tasks;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Tasks.CreateTask
{
    /// <summary>
    /// Handler for Task creation
    /// </summary>
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, Operation>
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly ITaskRepository _taskRepo;

        private readonly ITokenService _tokenService;
        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public CreateTaskHandler(IServiceProvider provider)
        {
            _taskBusiness = provider.GetService<ITaskBusiness>();
            _taskRepo = provider.GetService<ITaskRepository>();

            _tokenService = provider.GetService<ITokenService>();
            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var createModel = _mapper.Map<CreateTaskModel>(request);
            createModel.UserId = _tokenService.GetToken().Id;

            var newTaskResult = await _taskBusiness.Create(createModel);

            if(!newTaskResult.Success)
                return Operation.MakeFailure(newTaskResult.Message);  

            await _uow.Begin();

            await _taskRepo.Create(newTaskResult.Content);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}