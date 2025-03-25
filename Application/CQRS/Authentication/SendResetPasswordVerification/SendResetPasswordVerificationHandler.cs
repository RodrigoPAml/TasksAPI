using MediatR;
using Domain.Base;
using Domain.Enums;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Application.Interfaces.Email;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Authentication.SendResetPasswordVerification
{
    /// <summary>
    /// Handler for sending password reset verification code
    /// </summary>
    public class SendResetPasswordVerificationHandler : IRequestHandler<SendResetPasswordVerificationCommand, Operation>
    {
        private readonly IVerificationCodeRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;

        private readonly IUnityOfWork _uow;

        public SendResetPasswordVerificationHandler(IServiceProvider provider)
        {
            _repo = provider.GetService<IVerificationCodeRepository>();
            _userRepo = provider.GetService<IUserRepository>();
            _emailService = provider.GetService<IEmailService>();

            _uow = provider.GetService<IUnityOfWork>();
        }

        public async Task<Operation> Handle(SendResetPasswordVerificationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var newCodeResult = VerificationCode.Create(request.Email, ConfirmationCodeTypeEnum.Password);

            if (!newCodeResult.Success)
                return Operation.MakeFailure(newCodeResult.Message);

            if (!await _userRepo.ExistsByEmail(request.Email))
                return Operation.MakeFailure("Email not found");

            await _uow.Begin();

            await _repo.DeleteAllByEmail(request.Email, ConfirmationCodeTypeEnum.Password);
            await _uow.Save();

            var entity = newCodeResult.Content;
            await _repo.Create(entity);

            await _uow.Save();
            await _uow.Commit();

            await _emailService.SendEmail(
                request.Email,
                "Password Verification Code",
                $"Your password verification code is {entity.Code} and is valid for 15 minutes. Previous codes are no longer valid."
            );

            return Operation.MakeSuccess();
        }
    }
}