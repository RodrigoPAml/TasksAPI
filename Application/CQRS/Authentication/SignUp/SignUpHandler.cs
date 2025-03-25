using MediatR;
using Domain.Base;
using Domain.Utils;
using Domain.Enums;
using AutoMapper;
using Domain.Models.Users;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Application.Interfaces.Security;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Authentication.SignUp
{
    /// <summary>
    /// Handler for account creation
    /// </summary>
    public class SignUpHandler : IRequestHandler<SignUpCommand, Operation>
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IHashService _hashService;
        private readonly IUserRepository _userRepo;
        private readonly IVerificationCodeRepository _confirmationCodeRepository;

        private readonly IUnityOfWork _uow;
        private readonly IMapper _mapper;

        public SignUpHandler(IServiceProvider provider)
        {
            _userBusiness = provider.GetService<IUserBusiness>();
            _hashService = provider.GetService<IHashService>();
            _userRepo = provider.GetService<IUserRepository>();
            _confirmationCodeRepository = provider.GetService<IVerificationCodeRepository>();

            _uow = provider.GetService<IUnityOfWork>();
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<Operation> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var emailValidation = EmailUtils.ValidateEmail(request.Email);

            if (!emailValidation.Success)
                return emailValidation;

            if (await _userRepo.ExistsByEmail(request.Email))
                return Operation.MakeFailure("Email already in use");

            var confirmation = await _confirmationCodeRepository
                .GetByEmail(request.Email, ConfirmationCodeTypeEnum.Email);

            if (confirmation == null ||
               confirmation.Code != request.VerificationCode ||
               (DateTime.UtcNow - confirmation.Date).Minutes > 15
            )
            {
                return Operation.MakeFailure("Invalid verification code");
            }

            var createModel = _mapper.Map<CreateUserModel>(request);
            createModel.HashedPassword = _hashService.Hash(createModel.Password);

            var newUserResult = await _userBusiness.Create(createModel);

            if (!newUserResult.Success)
                return Operation.MakeFailure(newUserResult.Message);

            await _uow.Begin();

            await _userRepo.Create(newUserResult.Content);
            await _uow.Save();

            await _confirmationCodeRepository
                .DeleteAllByEmail(newUserResult.Content.Email, ConfirmationCodeTypeEnum.Email);
            
            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}