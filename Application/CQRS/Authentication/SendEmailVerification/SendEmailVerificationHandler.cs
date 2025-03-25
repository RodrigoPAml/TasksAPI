using MediatR;
using Domain.Base;
using Domain.Enums;
using Domain.Utils;
using Domain.Interfaces.Repositories;
using Application.Interfaces.Email;
using Application.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;

namespace Application.CQRS.Authentication.SendEmailVerification
{
    /// <summary>
    /// Handler for sending email verification code for account creation
    /// </summary>
    public class SendEmailVerificationHandler : IRequestHandler<SendEmailVerificationCommand, Operation>
    {
        private readonly IVerificationCodeRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;

        private readonly IUnityOfWork _uow;

        public SendEmailVerificationHandler(IServiceProvider provider)
        {
            _repo = provider.GetService<IVerificationCodeRepository>();
            _userRepo = provider.GetService<IUserRepository>();
            _emailService = provider.GetService<IEmailService>();

            _uow = provider.GetService<IUnityOfWork>();
        }

        public async Task<Operation> Handle(SendEmailVerificationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Operation.MakeFailure("Invalid request");

            var newCodeResult = VerificationCode.Create(request.Email, ConfirmationCodeTypeEnum.Email);

            if (!newCodeResult.Success)
                return Operation.MakeFailure(newCodeResult.Message);

            if (await _userRepo.ExistsByEmail(request.Email))
                return Operation.MakeFailure("Email already in use");

            await _uow.Begin();

            await _repo.DeleteAllByEmail(request.Email, ConfirmationCodeTypeEnum.Email);
            await _uow.Save();

            var entity = newCodeResult.Content;
            await _repo.Create(entity);

            await _uow.Save();
            await _uow.Commit();

            await _emailService.SendEmail(
                request.Email,
                "Email Verification Code",
                $"Your email verification code is {entity.Code} and is valid for 15 minutes. Previous codes are no longer valid."
            );

            return Operation.MakeSuccess();
        }
    }
}