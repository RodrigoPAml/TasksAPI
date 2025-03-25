using Domain.Models.Users;
using Domain.Base;
using Domain.Entities;

namespace Domain.Interfaces.Business
{
    /// <summary>
    /// Business Service interface for User entity
    /// </summary>
    public interface IUserBusiness
    {
        public Task<Result<User>> Create(CreateUserModel model);
        public Operation UpdatePassword(UpdateUserPasswordModel model);
        public Operation UpdateUsername(UpdateUsernameModel model);
    }
}
