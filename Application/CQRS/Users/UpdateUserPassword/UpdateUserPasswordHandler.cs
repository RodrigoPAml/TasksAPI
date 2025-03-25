using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Models.Users;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Security;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Users.UpdateUserPassword
{
    /// <summary>
    /// Handler for update User password
    /// </summary>
    public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, Operation>
    {
        private readonly IUserBusiness _userBusiness;
        private readonly ITokenService _tokenService;
        private readonly IHashService _hashService;
        private readonly IUserRepository _userRepo;

        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateUserPasswordHandler(IServiceProvider provider)
        {
            _userBusiness = provider.GetService<IUserBusiness>();
            _tokenService = provider.GetService<ITokenService>();
            _hashService = provider.GetService<IHashService>();
            _userRepo = provider.GetService<IUserRepository>();

            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var currentUserId = _tokenService.GetToken().Id;

            var updatePasswordModel = _mapper.Map<UpdateUserPasswordModel>(request);
            updatePasswordModel.User = await _userRepo.GetById(currentUserId);

            if (updatePasswordModel.User == null)
                return Operation.MakeFailure("User not found");

            if (!_hashService.IsValid(request.CurrentPassword, updatePasswordModel.User.HashedPassword))
                return Operation.MakeFailure("Invalid password");

            updatePasswordModel.NewPasswordHashed = _hashService.Hash(request.NewPassword);
            var updatePasswordResult = _userBusiness.UpdatePassword(updatePasswordModel);

            if (!updatePasswordResult.Success)
                return updatePasswordResult;

            await _uow.Begin();

            await _userRepo.UpdatePassword(currentUserId, updatePasswordModel.NewPasswordHashed);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}