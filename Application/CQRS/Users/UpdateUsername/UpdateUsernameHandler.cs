using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Models.Users;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Users.UpdateUsername
{
    /// <summary>
    /// Handler for update Username
    /// </summary>
    public class UpdateUsernameHandler : IRequestHandler<UpdateUsernameCommand, Operation>
    {
        private readonly IUserBusiness _userBusiness;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepo;

        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateUsernameHandler(IServiceProvider provider)
        {
            _userBusiness = provider.GetService<IUserBusiness>();
            _tokenService = provider.GetService<ITokenService>();
            _userRepo = provider.GetService<IUserRepository>();

            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var currentUserId = _tokenService.GetToken().Id;

            var updateUsernameModel = _mapper.Map<UpdateUsernameModel>(request);
            updateUsernameModel.User = await _userRepo.GetById(currentUserId);

            if (updateUsernameModel.User == null)
                return Operation.MakeFailure("User not found");

            var updateUsernameResult = _userBusiness.UpdateUsername(updateUsernameModel);

            if(!updateUsernameResult.Success)
                return updateUsernameResult;

            await _uow.Begin();

            await _userRepo.UpdateUsername(currentUserId, updateUsernameModel.User.Username);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}