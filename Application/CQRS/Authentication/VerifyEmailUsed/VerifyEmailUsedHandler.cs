using MediatR;
using Domain.Base;
using Domain.Utils;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Authentication.VerifyEmailUsed
{
    /// <summary>
    /// Handler for verify if an email is in use
    /// </summary>
    public class VerifyEmailUsedHandler : IRequestHandler<VerifyEmailUsedQuery, Result<bool>>
    {
        private readonly IUserRepository _userRepo;

        public VerifyEmailUsedHandler(IServiceProvider provider)
        {
            _userRepo = provider.GetService<IUserRepository>();
        }

        public async Task<Result<bool>> Handle(VerifyEmailUsedQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Result.MakeFailure<bool>("Invalid request");

            var emailValidation = EmailUtils.ValidateEmail(request.Email);

            if (!emailValidation.Success)
                return Result.MakeFailure<bool>("Invalid email");

            return Result.MakeSuccess(await _userRepo.ExistsByEmail(request.Email));
        }
    }
}