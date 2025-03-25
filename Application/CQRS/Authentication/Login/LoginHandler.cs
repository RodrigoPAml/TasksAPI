using MediatR;
using Domain.Base;
using Domain.Enums;
using Domain.Models.Token;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Authentication.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepo;

        private readonly LoginValidator _validator;

        public LoginHandler(IServiceProvider provider)
        {
            _tokenService = provider.GetService<ITokenService>();
            _userRepo = provider.GetService<IUserRepository>();

            _validator = provider.GetService<LoginValidator>();
        }

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if (!validation.Success)
                return Result.MakeFailure<string>(validation.Message);

            var user = await _userRepo.GetByEmail(request.Email);

            if (user == null)
                return Result.MakeFailure<string>("Failed to login");

            var token = _tokenService
                .CreateToken(
                    new TokenModel()
                    {
                        Id = user.Id,
                        Email = request.Email,
                        Username = user.Username,
                        Profile = user.Profile,
                    },
                    DateTime.UtcNow.AddHours(1)
                );

            return Result.MakeSuccess(token);
        }
    }
}