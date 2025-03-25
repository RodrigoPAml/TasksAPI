using Domain.Base;
using Domain.Enums;
using Domain.Entities;
using Domain.Models.Users;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Business
{
    /// <summary>
    /// Business service implementation for User entity
    /// </summary>
    public class UserBusiness : BaseBusiness, IUserBusiness
    {
        public UserBusiness(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<Result<User>> Create(CreateUserModel model)
        {
            if (model == null)
                return Result.MakeFailure<User>("Model is null");

            var newUserResult = User.Create(
                model.Email,
                model.Username,
                model.Password,
                model.HashedPassword,
                ProfileTypeEnum.User
            );

            if(!newUserResult.Success)
                return newUserResult;

            var repo = _provider.GetService<IUserRepository>();

            if(await repo.ExistsByEmail(model.Email))
                return Result.MakeFailure<User>("Email already used");

            return newUserResult;
        }

        public Operation UpdatePassword(UpdateUserPasswordModel model)
        {
            if (model == null)
                return Operation.MakeFailure("Model is null");

            if (model.User == null)
                return Operation.MakeFailure("User not found");

            return model.User.ChangePassword(model.NewPassword, model.NewPasswordHashed);
        }

        public Operation UpdateUsername(UpdateUsernameModel model)
        {
            if (model == null)
                return Operation.MakeFailure("Model is null");

            if (model.User == null)
                return Operation.MakeFailure("User not found");

            return model.User.ChangeUsername(model.NewUsername);
        }
    }
}
