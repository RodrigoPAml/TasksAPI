using MediatR;
using Domain.Base;
using Domain.Utils;
using Domain.Enums;
using Domain.Models.Users;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Application.Interfaces.Security;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Authentication.ResetPassword
{
    /// <summary>
    /// Handler for password reset
    /// </summary>
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Operation>
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IHashService _hashService;
        private readonly IUserRepository _userRepo;
        private readonly IVerificationCodeRepository _confirmationCodeRepository;

        private readonly IUnityOfWork _uow;

        public ResetPasswordHandler(IServiceProvider provider)
        {
            _userBusiness = provider.GetService<IUserBusiness>();
            _hashService = provider.GetService<IHashService>();
            _userRepo = provider.GetService<IUserRepository>();
            _confirmationCodeRepository = provider.GetService<IVerificationCodeRepository>();

            _uow = provider.GetService<IUnityOfWork>();
        }

        public async Task<Operation> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var emailValidation = EmailUtils.ValidateEmail(request.Email);

            if (!emailValidation.Success)
                return emailValidation;

            if (!await _userRepo.ExistsByEmail(request.Email))
                return Operation.MakeFailure("Invalid email");

            var confirmation = await _confirmationCodeRepository
                .GetByEmail(request.Email, ConfirmationCodeTypeEnum.Password);

            if(confirmation == null ||
               confirmation.Code != request.VerificationCode ||
               (DateTime.UtcNow - confirmation.Date).Minutes > 15
            )
            {
                return Operation.MakeFailure("Invalid verification code");
            }

            var user = await _userRepo.GetByEmail(request.Email);

            var passwordValidation = _userBusiness.UpdatePassword(new UpdateUserPasswordModel()
            {
                User = user,
                NewPassword = request.NewPassword,
                NewPasswordHashed = _hashService.Hash(request.NewPassword)
            });

            await _uow.Begin();

            await _userRepo.UpdatePassword(user.Id, user.HashedPassword);
            await _uow.Save();

            await _confirmationCodeRepository
                .DeleteAllByEmail(request.Email, ConfirmationCodeTypeEnum.Password);
            
            await _uow.Save();
            await _uow.Commit();

            return Operation.MakeSuccess();
        }
    }
}