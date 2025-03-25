using Infra.Base;
using Domain.Enums;
using Domain.Entities;
using Infra.Entities;
using Infra.Contexts;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infra.Implementations.Repositories
{
    /// <summary>
    /// Repository implementation for the Verification Code entity
    /// </summary>
    public class VerificationCodeRepository : Repository<DbVerificationCode>, IVerificationCodeRepository
    {
        public VerificationCodeRepository(DatabaseContext ctx, IServiceProvider provider) : base(ctx.VerificationCodes, provider)
        {
        }

        public async Task<VerificationCode> GetByEmail(string email, ConfirmationCodeTypeEnum type)
        {
            var entity = await _dbSet
                .Where(x => x.Email == email && x.Type == (int)type)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            return _mapper.Map<VerificationCode>(entity);
        }

        public async Task Create(VerificationCode entity)
        {
            var dbEntity = _mapper.Map<DbVerificationCode>(entity);
            await CreateAsync(dbEntity);
        }

        public async Task DeleteAllByEmail(string email, ConfirmationCodeTypeEnum type)
        {
            var ids = _dbSet
                .Where(x => x.Email == email && x.Type == (int)type)
                .AsNoTrackingWithIdentityResolution()
                .Select(x => x.Id)
                .ToList();

            await Task.WhenAll(ids.Select(x => DeleteAsync(x)));
        }
    }
}