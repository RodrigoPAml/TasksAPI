using MediatR;
using Domain.Base;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces.Authentication;
using Application.Interfaces.Security;

namespace Application.CQRS.Users.DeleteUser
{
    /// <summary>
    /// Handler for update Username
    /// </summary>
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Operation>
    {
        private readonly ITokenService _tokenService;
        private readonly IHashService _hashService;

        private readonly IUserRepository _userRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ITaskRepository _taskRepo;

        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public DeleteUserHandler(IServiceProvider provider)
        {
            _tokenService = provider.GetService<ITokenService>();
            _hashService = provider.GetService<IHashService>();

            _userRepo = provider.GetService<IUserRepository>();
            _categoryRepo = provider.GetService<ICategoryRepository>();
            _taskRepo = provider.GetService<ITaskRepository>();

            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var currentUserId = _tokenService.GetToken().Id;
            var user = await _userRepo.GetById(currentUserId);

            if(user == null)
                return Operation.MakeFailure("User not found");

            if (!_hashService.IsValid(request.CurrentPassword, user.HashedPassword))
                return Operation.MakeFailure("Invalid password");

            await _uow.Begin();

            var tasks = await _taskRepo.GetByUser(currentUserId);
            await Task.WhenAll(tasks.Select(x => _taskRepo.Delete(x.Id)));

            await _uow.Save();
             
            var categories = await _categoryRepo.GetByUser(currentUserId);
            await Task.WhenAll(categories.Select(x => _categoryRepo.Delete(x.Id)));

            await _uow.Save();

            await _userRepo.Delete(currentUserId);

            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}