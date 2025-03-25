using Domain.Entities;
using Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Verification Code entity
    /// </summary
    public interface IVerificationCodeRepository
    {
        public Task<VerificationCode> GetByEmail(string email, ConfirmationCodeTypeEnum type);
        public Task Create(VerificationCode entity);
        public Task DeleteAllByEmail(string email, ConfirmationCodeTypeEnum type);
    }
}