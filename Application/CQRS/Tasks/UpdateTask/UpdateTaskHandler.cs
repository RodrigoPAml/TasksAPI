using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Models.Tasks;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Tasks.UpdateTask
{
    /// <summary>
    /// Handler for Task update
    /// </summary>
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, Operation>
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly ITaskRepository _taskRepo;

        private readonly ITokenService _tokenService;
        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateTaskHandler(IServiceProvider provider)
        {
            _taskBusiness = provider.GetService<ITaskBusiness>();
            _taskRepo = provider.GetService<ITaskRepository>();

            _tokenService = provider.GetService<ITokenService>();
            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var userId = _tokenService.GetToken().Id;

            var updateModel = new UpdateTaskModel(); 
            updateModel.Task = await _taskRepo.GetById(request.Id);
            updateModel.Info = _mapper.Map<UpdateTaskInfoModel>(request);

            if (updateModel.Task != null && updateModel.Task.UserId != userId)
                return Operation.MakeFailure("Not Allowed");

            var updateResult = await _taskBusiness.Update(updateModel);

            if(!updateResult.Success)
                return updateResult;  

            await _uow.Begin();

            await _taskRepo.Update(updateModel.Task);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}