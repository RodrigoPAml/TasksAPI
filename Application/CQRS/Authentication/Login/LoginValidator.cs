using Domain.Base;
using Domain.Interfaces.Repositories;
using Application.Interfaces.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Authentication.Login
{
    public class LoginValidator : ValidatorAsync<LoginCommand>
    {
        private readonly IServiceProvider _provider;

        public LoginValidator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override async Task<Operation> ValidateAsync(LoginCommand model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return Operation.MakeFailure("Failed to login");

            if (string.IsNullOrEmpty(model.Password))
                return Operation.MakeFailure("Failed to login");

            var userRepo = _provider.GetService<IUserRepository>();

            if (!await userRepo.ExistsByEmail(model.Email))
                return Operation.MakeFailure("Failed to login");

            var user = await userRepo.GetByEmail(model.Email);

            if (user == null)
                return Operation.MakeFailure("Failed to login");

            var hasher = _provider.GetService<IHashService>();

            if (!hasher.IsValid(model.Password, user.HashedPassword))
                return Operation.MakeFailure("Failed to login");

            return Operation.MakeSuccess();
        }
    }
}
