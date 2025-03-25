using Infra.Base;
using Infra.Entities;
using Domain.Entities;
using Infra.Contexts;
using Infra.Implementations.Query;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infra.Implementations.Repositories
{
    /// <summary>
    /// Repository implementation for the User entity
    /// </summary>
    public class UserRepository : Repository<DbUser>, IUserRepository
    {
        public UserRepository(DatabaseContext ctx, IServiceProvider provider) : base(ctx.Users, provider)
        {
        }
       
        public async Task<bool> ExistsByEmail(string email)
        {
            return await _dbSet.AnyAsync(x => x.Email == email );
        }

        public new async Task<bool> ExistsById(int id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<User> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var filter = new Filter<DbUser>(x => x.Email == email);

            var entity = await _dbSet
                .AsNoTrackingWithIdentityResolution()
                .Where(filter.GetExpression())
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            return _mapper.Map<User>(entity);
        }

        public new async Task<User> GetById(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
                return null;

            return _mapper.Map<User>(entity);
        }

        public async Task Create(User user)
        {
            var entity = _mapper.Map<DbUser>(user);
            await CreateAsync(entity);
        }

        public async Task UpdatePassword(int id, string password)
        {
            // Caution, tracked entity with missing fields
            await UpdateSomeFieldsAsync(new DbUser()
                {
                    Id = id,
                    Password = password
                },
                new Fields<DbUser>(x => x.Password)
            );
        }

        public async Task UpdateUsername(int id, string username)
        {
            // Caution, tracked entity with missing fields
            await UpdateSomeFieldsAsync(new DbUser()
                {
                    Id = id,
                    Username = username
                },
                new Fields<DbUser>(x => x.Username)
            );
        }

        public new async Task Delete(int id)
        {
            await DeleteAsync(id);
        }
    }
}